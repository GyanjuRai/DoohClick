

using DoohClick.DataAccess.Dapper;
using DoohClick.DataAccess.Data;
using DoohClick.Interface.Shared.Account;
using DoohClick.Interface.Shared.Auth;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Shared.Account;
using DoohClick.Model.Shared.AppClaim;
using DoohClick.Model.Shared.Auth;
using DoohClick.Model.Shared.Exceptions.Auth;
using DoohClick.Model.Shared.Exceptions.NotFound;
using DoohClick.Model.Shared.Exceptions.Validation;
using DoohClick.Service.Shared.Base;
using DoohClick.Service.Shared.Helper.HashingHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace DoohClick.Service.Shared.Account
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;
        private readonly MvJwtConfig _jwtConfig;
        public AccountService(
                IDataAccessService dataAccessService,
                IJsonSerializer jsonSerializer,
                IAuthService authService,
                MvJwtConfig jwtConfig,
                AppDbContext context
            ) : base(dataAccessService, jsonSerializer)
        {
            _authService = authService;
            _context = context;
            _jwtConfig = jwtConfig;
        }

        public async Task<MvLoginResponse> Login(MvLoginInfoParam param)
        {
            string message = "Invalid credentials";
            string loginInfoResult = await _dataAccessService.RetrievalProcedure("identity.sp_user_login_info_sel", JsonConvert.SerializeObject(param));

            MvLoginInfoResponse? loginInfo = _jsonSerializer.DeserializeObject<MvLoginInfoResponse>(loginInfoResult);

            if (loginInfo is null || string.IsNullOrEmpty(loginInfo.UserUuid))
            {
                throw new NotFoundException(
                    message: message
                );
            }

            if (!EncryptionHelper.VerifyPassword(param.Password, loginInfo.PasswordHash))
            {
                throw new ValidationException(
                    message: message
                );
            }

            MvUserInfoParam userInfoParam = new MvUserInfoParam
            {
                UserUuid = loginInfo.UserUuid,
                TenantCode = loginInfo.TenantCode
            };
            MvUserInfoResponse? userInfoResult = await GetUserInfo(userInfoParam);
            if (userInfoResult is null)
            {
                throw new NotFoundException(
                    message: message
                );
            }

            Claim[] claims =
            [
                new Claim(AppClaim.UserId, userInfoResult.UserId.ToString()),
                new Claim(AppClaim.UserUuuId, userInfoResult.UserUuid),
                new Claim(AppClaim.TenantId, userInfoResult.TenantId.ToString()),
                new Claim(AppClaim.TenantCode, userInfoResult.TenantCode),
                new Claim(AppClaim.FullName, userInfoResult.FullName),
                new Claim(AppClaim.UserRole, userInfoResult.UserRole),
                new Claim(AppClaim.Email, userInfoResult.Email)
            ];
            MvJwtResult token = await _authService.GenerateAccessToken(claims);
         
            await SyncRefreshToken(BuildRefreshTokenParam(userInfoResult.UserId, token));

            return BuildLoginResponse(token);
        }

        public async Task<MvLoginResponse> RefreshToken(MvRefreshTokenParam param)
        {
            string message = "Session has expired";
            MvRefreshToken? refreshToken = await _context.Users
                .Where(u => u.Id == param.UserId)
                .Select(u => new MvRefreshToken
                {
                    RefreshToken = u.RefreshToken,
                    RefreshTokenExpiry = u.RefreshTokenExpiry
                })
                .FirstOrDefaultAsync();

            if (refreshToken is null)
            {
                throw new AuthException(
                    message: message
                );
            }

            if (refreshToken.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new AuthException(
                    message: message
                );
            }

            ClaimsPrincipal principal = _authService.GetPrincipalFromExpiredToken(param.AccessToken);

            int UserId = int.TryParse(principal.FindFirst(AppClaim.UserId)?.Value, out int userId)
                                     ? userId
                                     : throw new AuthException(message: message);
            MvJwtResult token = await _authService.GenerateAccessToken(principal.Claims.ToArray());

            await SyncRefreshToken(BuildRefreshTokenParam(UserId, token));

            return BuildLoginResponse(token);
        }

        public async Task<MvUserInfoResponse?> GetUserInfo(MvUserInfoParam param)
        {
            string result = await _dataAccessService.RetrievalProcedure("identity.sp_user_info_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvUserInfoResponse>(result);
        }

        private async Task SyncRefreshToken(MvRefreshTokenSyncParam param)
        {
            await _dataAccessService.ActionProcedure("identity.sp_user_refresh_token_upd", JsonConvert.SerializeObject(param));
        }

        private MvRefreshTokenSyncParam BuildRefreshTokenParam(int userId, MvJwtResult token)
        {
            return new MvRefreshTokenSyncParam
            {
                UserId = userId,
                RefreshToken = token.RefreshToken,
                RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtConfig.RefreshTokenExpirationMin)
            };
        }
        private MvLoginResponse BuildLoginResponse(MvJwtResult token)
        {
            return new MvLoginResponse
            {
                AccessToken = token.AccessToken,
                ExpireAt = token.ExpireAt,
                RefreshToken = token.RefreshToken
            };
        }
    }
}
