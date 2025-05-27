namespace Client.Models.Categories
{
    public sealed record CategoryItemResponse
    {
        public CategoryItemResponse()
        {

        }
        
        public int CategoryId { get; init; }
        public string Title { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}