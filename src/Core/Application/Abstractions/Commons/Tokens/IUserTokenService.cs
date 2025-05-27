namespace Application.Abstractions.Commons.Tokens
{
    public interface IUserTokenService
    {
        public int UserId { get; }
        public string UserEmail { get; }
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }
    }
}
