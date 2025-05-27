namespace Application.Models.DTOs.Users
{
    public sealed record UserItemDto
    {
        public UserItemDto()
        {
            
        }

        public UserItemDto(int userId, string email, string firstName, string lastName, DateTime createdDate, DateTime updatedDate, bool isActive)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            IsActive = isActive;
        }

        public int UserId { get; init; }
        public string Email { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; set; }
    }
}