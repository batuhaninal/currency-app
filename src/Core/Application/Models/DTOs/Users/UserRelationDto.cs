namespace Application.Models.DTOs.Users
{
    public sealed record UserRelationDto
    {
        public UserRelationDto()
        {
            
        }

        public UserRelationDto(int userId, string email, string firstName, string lastName)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public int UserId { get; init; }
        public string Email { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
    }
}