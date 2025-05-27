namespace Application.Models.DTOs.Categories
{
    public sealed record CreateCategoryDto
    {
        public CreateCategoryDto()
        {
            
        }
        public CreateCategoryDto(string title, bool isActive)
        {
            Title = title.TrimStart().TrimEnd();
            IsActive = isActive;
        }
        
        public string Title { get; init; } = null!;
        public bool IsActive { get; set; }
    }
}