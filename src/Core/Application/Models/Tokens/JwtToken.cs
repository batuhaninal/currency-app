namespace Application.Models.Tokens
{
    public class JwtToken
    {
        public string AccessToken { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
