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

    public sealed record CurrencyTagToolResponse
    {
        public CurrencyTagToolResponse()
        {

        }

        public CurrencyTagToolResponse(int currencyTagId, int currencyId, string value)
        {
            CurrencyTagId = currencyTagId;
            CurrencyId = currencyId;
            Value = value;
        }

        public int CurrencyTagId { get; init; }
        public int CurrencyId { get; init; }
        public string Value { get; init; } = null!;
    }

    public sealed record CurrencyTagInfoResponse
    {
        public CurrencyTagInfoResponse()
        {
            
        }

        public CurrencyTagInfoResponse(int currencyTagId, string value, CurrencyRelationResponse? currency)
        {
            CurrencyTagId = currencyTagId;
            Value = value;
            Currency = currency;
        }

        public int CurrencyTagId { get; init; }
        public string Value { get; init; }
        public CurrencyRelationResponse? Currency { get; init; }
    }
}