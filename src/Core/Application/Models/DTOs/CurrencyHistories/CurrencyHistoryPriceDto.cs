namespace Application.Models.DTOs.CurrencyHistories
{
    public record CurrencyHistoryPriceDto
    {
        public CurrencyHistoryPriceDto()
        {

        }

        public CurrencyHistoryPriceDto(int currencyHistoryId, int currencyId, decimal oldPurchasePrice, decimal newPurchasePrice, decimal oldSalePrice, decimal newSalePrice)
        {
            CurrencyHistoryId = currencyHistoryId;
            CurrencyId = currencyId;
            OldPurchasePrice = oldPurchasePrice;
            NewPurchasePrice = newPurchasePrice;
            OldSalePrice = oldSalePrice;
            NewSalePrice = newSalePrice;
        }

        public int CurrencyHistoryId { get; init; }
        public int CurrencyId { get; init; }
        public decimal OldPurchasePrice { get; init; }
        public decimal NewPurchasePrice { get; init; }
        public decimal OldSalePrice { get; init; }
        public decimal NewSalePrice { get; init; }
    }
}