using Application.Models.Constants.Messages;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.Users;

namespace Application.Models.DTOs.Assets
{
    public sealed record AssetInfoDto
    {
        public AssetInfoDto()
        {
            
        }

        public AssetInfoDto(int assetId, int count, decimal purchasePrice, decimal salePrice, decimal currentPurchasePrice, decimal currentSalePrice, DateOnly purchaseDate, DateTime createdDate, DateTime updatedDate, bool isActive, CurrencyRelationDto? currency = null, UserRelationDto? user = null)
        {
            AssetId = assetId;
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CurrentPurchasePrice = currentPurchasePrice;
            CurrentSalePrice = currentSalePrice;
            PurchaseDate = purchaseDate;
            CreatedDate = createdDate; 
            UpdatedDate = updatedDate;
            IsActive = isActive;
            Currency = currency ?? new CurrencyRelationDto(0, ErrorMessage.PASSIVEorDELETED, null, 0, 0);
            User = user ?? new UserRelationDto(0, ErrorMessage.PASSIVEorDELETED, "", "");
        }

        public int AssetId { get; init; }
        public int Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public decimal CurrentPurchasePrice { get; init; }
        public decimal CurrentSalePrice { get; init; }
        public CurrencyRelationDto Currency { get; init; }
        public UserRelationDto User { get; init; }
        public DateOnly PurchaseDate { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
    }
}