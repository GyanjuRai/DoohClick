
using DoohClick.Interface.Shared.Auth;
using DoohClick.Model.Shared.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DoohClick.Service.Shared.Auth
{
    public class AuthService : IAuthService
    {
        private readonly MvJwtConfig _jwtConfig;
        private readonly byte[] _secret;
        public AuthService(
               MvJwtConfig jwtConfig
            )
        {
            _jwtConfig = jwtConfig;
            _secret = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
        }

        public async Task<MvJwtResult> GenerateAccessToken(Claim[]? claims)
        {
            bool shouldAddAudienceClaim = string.IsNullOrEmpty(claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud)?.Value);
            SigningCredentials creds = new(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                    issuer: _jwtConfig.Issuer,
                    audience: shouldAddAudienceClaim ? _jwtConfig.Audience : null,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpirationMin),
                    signingCredentials: creds
                );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            string refreshToken = GenerateRefreshToken();
            return await Task.FromResult(new MvJwtResult
            {
                AccessToken = accessToken,
                ExpireAt = token.ValidTo,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtConfig.RefreshTokenExpirationMin)
            });
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secret),
                ValidateAudience = true,
                ValidAudience = _jwtConfig.Audience,
                ValidateLifetime = false, // We want to get claims from expired token
                ClockSkew = TimeSpan.FromMinutes(_jwtConfig.AccessTokenClockSkewMin)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
