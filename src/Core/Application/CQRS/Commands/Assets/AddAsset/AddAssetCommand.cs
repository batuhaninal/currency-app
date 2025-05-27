using Application.CQRS.Commons.Interfaces;
using Domain;

namespace Application.CQRS.Commands.Assets.AddAsset
{
    public sealed record AddAssetCommand : ICommand
    {
        public AddAssetCommand()
        {

        }

        public AddAssetCommand(int currencyId, int count, decimal purchasePrice, decimal salePrice, DateOnly purchaseDate)
        {
            CurrencyId = currencyId;
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            PurchaseDate = purchaseDate;
        }

        public int CurrencyId { get; init; }
        public int Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public DateOnly PurchaseDate { get; init; }

        internal Asset ToDomain(int userId, decimal currentPurchasePrice, decimal currentSalePrice)
        {
            DateTime now = DateTime.UtcNow;
            return new Asset
            {
                CurrencyId = this.CurrencyId,
                Count = Count,
                PurchasePrice = this.PurchasePrice,
                SalePrice = this.SalePrice,
                CreatedDate = now,
                UpdatedDate = now,
                IsActive = true,
                PurchaseDate = this.PurchaseDate,
                UserId = userId,
                CurrentPurchasePrice = currentPurchasePrice,
                CurrentSalePrice = currentSalePrice
            };
        }
    }
}