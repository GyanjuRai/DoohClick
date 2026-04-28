

namespace DoohClick.Model.Shared.Auth
{
    public record MvJwtConfig
    {
        public required string Secret { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required int AccessTokenExpirationMin { get; set; }
        public required int RefreshTokenExpirationMin { get; set; }
        public required int AccessTokenClockSkewMin { get; set; } // Optional: Clock skew in minutes to account for time synchronization issues
    }

    public record MvJwtResult
    {
        public required string AccessToken { get; set; }
        public required DateTime ExpireAt { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; } // Optional: Expiration time for the refresh token
    }
}
