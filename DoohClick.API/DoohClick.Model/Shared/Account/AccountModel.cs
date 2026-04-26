

namespace DoohClick.Model.Shared.Account
{

    public record MvLoginInfoParam
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

    public record MvLoginInfoResponse
    {
        public required string UserUuid { get; set; }
        public required string PasswordHash { get; set; }
        public required string TenantCode { get; set; }
    }

    public record MvUserInfoParam
    {
        public required string UserUuid { get; set; }
        public required string TenantCode { get; set; }
    }

    public record MvUserInfoResponse
    {
        public required int UserId { get; set; }
        public required string UserUuid { get; set; }
        public required int TenantId { get; set; }
        public required string TenantCode { get; set; }
        public required string FullName { get; set; }
        public required string UserRole { get; set; }
        public required string Email { get; set; }
    }

    public record MvRefreshTokenSyncParam
    {
        public required string RefreshToken { get; set; }
        public required int UserId { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }

    public record MvRefreshTokenParam
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required int UserId { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }

    public record MvRefreshToken
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }

    public record MvLoginResponse
    {
        public required string AccessToken { get; set; }
        public required DateTime ExpireAt { get; set; }
        public required string RefreshToken { get; set; }
    }
}
