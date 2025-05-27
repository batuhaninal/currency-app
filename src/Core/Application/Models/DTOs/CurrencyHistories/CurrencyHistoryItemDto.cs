namespace Application.Models.DTOs.CurrencyHistories
{
    public record CurrencyHistoryItemDto
    {
        public CurrencyHistoryItemDto()
        {

        }

        public CurrencyHistoryItemDto(int currencyHistoryId, int currencyId, decimal oldPurchasePrice, decimal newPurchasePrice, decimal oldSalePrice, decimal newSalePrice, DateOnly date, DateTime updatedDate)
        {
            CurrencyHistoryId = currencyHistoryId;
            CurrencyId = currencyId;
            OldPurchasePrice = oldPurchasePrice;
            NewPurchasePrice = newPurchasePrice;
            OldSalePrice = oldSalePrice;
            NewSalePrice = newSalePrice;
            Date = date;
            UpdatedDate = updatedDate;
        }

        public int CurrencyHistoryId { get; init; }
        public int CurrencyId { get; init; }
        public decimal OldPurchasePrice { get; init; }
        public decimal NewPurchasePrice { get; init; }
        public decimal OldSalePrice { get; init; }
        public decimal NewSalePrice { get; init; }
        public DateOnly Date { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}