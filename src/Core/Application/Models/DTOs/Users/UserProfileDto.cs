namespace Application.Models.DTOs.Users
{
    public record UserProfileDto
    {
        public UserProfileDto()
        {
            
        }

        public UserProfileDto(int userId, string firstName, string lastName, string email, string? role)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role ?? "Role bulunamadi!";
        }

        public int UserId { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Role { get; init; } = null!;
    }
}