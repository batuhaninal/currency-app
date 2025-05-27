namespace Application.Models.DTOs.Categories
{
    public sealed record CategoryItemDto
    {
        public CategoryItemDto()
        {
            
        }

        public CategoryItemDto(int categoryId, string title, bool isActive)
        {
            CategoryId = categoryId;
            Title = title;
            IsActive = isActive;
        }

        public int CategoryId { get; init; }
        public string Title { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}