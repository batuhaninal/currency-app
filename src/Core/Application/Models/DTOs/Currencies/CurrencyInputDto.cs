namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyInputDto
    {
        public CurrencyInputDto()
        {
            
        }

        public CurrencyInputDto(string title, decimal value, int categoryId, bool isActive)
        {
            Title = title.TrimStart().TrimEnd();
            Value = value;
            CategoryId = categoryId;
            IsActive = isActive;
        }

        public string Title { get; init; } = null!;
        public decimal Value { get; init; }
        public int CategoryId { get; init; }
        public bool IsActive { get; init; }
    }
}