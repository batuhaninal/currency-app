namespace Client.Models.Currencies
{
    public sealed record CurrencyInput
    {
        public CurrencyInput()
        {

        }

        public CurrencyInput(string title, string? subTitle, string? tVCode, string? xPath, decimal purchasePrice, decimal salePrice, int categoryId, bool isActive)
        {
            Title = title;
            SubTitle = subTitle;
            TvCode = tVCode;
            XPath = xPath;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CategoryId = categoryId;
            IsActive = isActive;
        }

        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public string? XPath { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public int CategoryId { get; init; }
        public bool IsActive { get; init; }
    }

    public sealed record CurrencyTagInput
    {
        public CurrencyTagInput()
        {

        }

        public CurrencyTagInput(int currencyId, string value)
        {
            CurrencyId = currencyId;
            Value = value;
        }


        public CurrencyTagInput(int currencyId)
        {
            CurrencyId = currencyId;
        }
        public int CurrencyId { get; init; }

        public string Value { get; init; } = null!;
    }
}