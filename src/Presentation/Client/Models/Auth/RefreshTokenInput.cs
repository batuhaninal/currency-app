namespace Client.Models.Auth
{
    public sealed record RefreshTokenInput
    {
        public RefreshTokenInput()
        {
            
        }

        public RefreshTokenInput(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; init; } = null!;

    }
}