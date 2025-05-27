namespace Client.Models.Categories
{
    public sealed record CategoryRelationResponse
    {
        public CategoryRelationResponse()
        {
        }

        public CategoryRelationResponse(int categoryId, string title)
        {
            CategoryId = categoryId;
            Title = title;
        }

        public int CategoryId { get; init; }
        public string Title { get; init; } = null!;
    }
}