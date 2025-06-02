namespace Client.Models.Currencies
{
    public sealed record CurrencyToolResponse
    {
        public CurrencyToolResponse()
        {

        }

        public CurrencyToolResponse(int currencyId, string title)
        {
            CurrencyId = currencyId;
            Title = title;
        }

        public int CurrencyId { get; init; }
        public string Title { get; init; } = null!;
    }
}