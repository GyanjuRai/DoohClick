

using DoohClick.Model.Shared.Auth;
using System.Security.Claims;

namespace DoohClick.Interface.Shared.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task<MvJwtResult> GenerateAccessToken(Claim[]? claims);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
