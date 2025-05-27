namespace Client.Models.Currencies
{
    public sealed record CurrencyInput
    {
        public CurrencyInput()
        {

        }

        public CurrencyInput(string title, string? subTitle, string? tVCode, decimal purchasePrice, decimal salePrice, int categoryId, bool isActive)
        {
            Title = title;
            SubTitle = subTitle;
            TvCode = tVCode;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CategoryId = categoryId;
            IsActive = isActive;
        }

        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public int CategoryId { get; init; }
        public bool IsActive { get; init; }
    }
}