namespace Application.Models.DTOs.Currencies
{
    public sealed record CurrencyToolDto
    {
        public CurrencyToolDto()
        {

        }

        public CurrencyToolDto(int currencyId, string title, string? subTitle, bool isActive)
        {
            CurrencyId = currencyId;
            Title = title;
            SubTitle = subTitle;
            IsActive = isActive;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; }
        public string? SubTitle { get; init; }
        public bool IsActive { get; init; }
    }
}