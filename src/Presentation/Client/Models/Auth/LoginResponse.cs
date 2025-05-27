namespace Client.Models.Auth
{
    public sealed record LoginResponse
    {
        public string AccessToken { get; init; } = null!;
        public string RefreshToken { get; init; } = null!;
        public DateTime Expiration { get; init; }
    }
}