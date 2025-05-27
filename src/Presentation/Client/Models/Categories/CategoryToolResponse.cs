namespace Client.Models.Categories
{
    public sealed record CategoryToolResponse
    {
        public CategoryToolResponse()
        {

        }

        public CategoryToolResponse(int categoryId, string title, bool isActive)
        {
            CategoryId = categoryId;
            Title = title;
            IsActive = isActive;
        }

        public int CategoryId { get; init; }
        public string Title { get; init; }
        public bool IsActive { get; init; }
    }
}