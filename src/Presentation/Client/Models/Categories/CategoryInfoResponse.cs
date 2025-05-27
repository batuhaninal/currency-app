namespace Client.Models.Categories
{
    public sealed record CategoryInfoResponse
    {
        public CategoryInfoResponse()
        {
            
        }
        public CategoryInfoResponse(int categoryId, string title, DateTime createdDate, DateTime updatedDate, bool isActive, int currencyCount)
        {
            CategoryId = categoryId;
            Title = title;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            IsActive = isActive;
            CurrencyCount = currencyCount;
        }

        public int CategoryId { get; init; }
        public string Title { get; init; } = null!;
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
        public int CurrencyCount { get; init; }
    }
}