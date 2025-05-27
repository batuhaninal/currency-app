namespace Application.Models.DTOs.Categories
{
    public sealed record CategoryRelationDto
    {
        public CategoryRelationDto()
        {
            
        }

        public CategoryRelationDto(int categoryId, string title)
        {
            CategoryId = categoryId;
            Title = title;
        }

        public int CategoryId { get; init; }
        public string Title { get; init; } = null!;
    }
}