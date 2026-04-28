using DoohClick.Model.Shared.AppClaim;
using DoohClick.Model.Shared.Exceptions.Auth;
using System.Security.Claims;

namespace DoohClick.API.Helper
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetTenantId(this ClaimsPrincipal user)
        {
            string? tenantIdClaim = user.FindFirst(AppClaim.TenantId)?.Value;
            if (tenantIdClaim == null || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                throw new AuthException("Session has expired.");
            }
            return tenantId;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            string? userIdClaim = user.FindFirst(AppClaim.UserId)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthException("Session has expired.");
            }
            return userId;
        }
    }
}
