namespace Client.Models.Auth
{
    public sealed record RegisterInput
    {
        public RegisterInput()
        {

        }

        public RegisterInput(string username, string password, string firstName, string lastName)
        {
            Username = username.TrimStart().TrimEnd();
            Password = password;
            FirstName = firstName.TrimStart().TrimEnd();
            LastName = lastName.TrimStart().TrimEnd();
        }

        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
    }
}