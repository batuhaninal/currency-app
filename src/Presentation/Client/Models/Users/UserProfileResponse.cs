namespace Client.Models.Users
{
    public sealed record UserProfileResponse
    {
        public UserProfileResponse()
        {
            
        }

        public UserProfileResponse(int userId, string firstName, string lastName, string email, string role)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
        }

        public int UserId { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Role { get; init; } = null!;
    }
}