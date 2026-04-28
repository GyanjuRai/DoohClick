

using DoohClick.DataAccess.Dapper;
using DoohClick.DataAccess.Data;
using DoohClick.Interface.Shared.Account;
using DoohClick.Interface.Shared.Auth;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Shared.Account;
using DoohClick.Model.Shared.AppClaim;
using DoohClick.Model.Shared.Auth;
using DoohClick.Model.Shared.Exceptions;
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
                throw new AuthException(
                    message: message,
                    statusCode: 401,
                    errorCode: "INVALID_CREDENTIALS"
                );
            }

            if (!EncryptionHelper.VerifyPassword(param.Password, loginInfo.PasswordHash))
            {
                throw new AuthException(
                    message: message,
                    statusCode: 401,
                    errorCode: "INVALID_CREDENTIALS"
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
                throw new AuthException(
                    message: message,
                    statusCode: 401,
                    errorCode: "USER_INFO_NOT_FOUND"
                );
            }

            Claim[] claims =
            [
                new Claim(AppClaim.UserId, userInfoResult.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userInfoResult.UserUuid),
                new Claim(AppClaim.TenantId, userInfoResult.TenantId.ToString()),
                new Claim(AppClaim.TenantCode, userInfoResult.TenantCode),
                new Claim(ClaimTypes.Name, userInfoResult.FullName),
                new Claim(ClaimTypes.Role, userInfoResult.UserRole),
                new Claim(ClaimTypes.Email, userInfoResult.Email)
            ];
            MvJwtResult token = await _authService.GenerateAccessToken(claims);
         
            await SyncRefreshToken(BuildRefreshTokenParam(userInfoResult.UserId, token));

            return BuildLoginResponse(token);
        }

        public async Task<MvLoginResponse> RefreshToken(MvRefreshTokenParam param)
        {
            MvRefreshToken? refreshToken = await _context.Users
                .Where(u => u.RefreshToken == param.RefreshToken)
                .Select(u => new MvRefreshToken
                {
                    RefreshToken = u.RefreshToken,
                    RefreshTokenExpiry = u.RefreshTokenExpiry
                })
                .FirstOrDefaultAsync();

            if (refreshToken is null)
            {
                throw new AuthException(
                    message: "Invalid refresh token",
                    statusCode: 401,
                    errorCode: "INVALID_REFRESH_TOKEN"
                );
            }

            if (refreshToken.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new AuthException(
                    message: "Refresh token has expired",
                    statusCode: 401,
                    errorCode: "EXPIRED_REFRESH_TOKEN"
                );
            }

            ClaimsPrincipal principal = _authService.GetPrincipalFromExpiredToken(param.AccessToken);

            int UserId = int.TryParse(principal.FindFirst(AppClaim.UserId)?.Value, out int userId)
                                     ? userId
                                     : throw new AuthException(message: "Invalid token claims", statusCode: 401, errorCode: "INVALID_TOKEN");
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
