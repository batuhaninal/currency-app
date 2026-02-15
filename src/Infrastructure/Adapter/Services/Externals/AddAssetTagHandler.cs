using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.AIs;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Services.Externals
{
    public sealed class AddAssetTagHandler : IAITagHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAssetTagHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AiAction AiAction => AiAction.AddAsset;

        public async Task<IBaseResult> HandleAsync(AiIntent intent, int userId, CancellationToken cancellationToken = default)
        {
            if(intent is null)
                return new ResultDto(400, false, null, "Intent is null");

            if(string.IsNullOrWhiteSpace(intent.AssetName))
                return new ResultDto(400, false, null, "Asset Name is required");

            if(intent.Amount is null || intent.Amount <= 0)
                return new ResultDto(400, false, null, "Amount must be greater than zero");

            string normalised = intent.AssetName.TrimStart().TrimEnd().ToLower();

            int currencyId = await _unitOfWork.
                CurrencyTagReadRepository.
                Table.
                AsNoTracking().
                Where(x=> x.Value.ToLower() == normalised).
                Select(x=> x.CurrencyId).
                FirstOrDefaultAsync(cancellationToken);

            if(currencyId == 0)
                return new ResultDto(400, false, null, "Asset name not found");

            Currency? currency = await _unitOfWork.CurrencyReadRepository.GetByIdAsync(currencyId, false, cancellationToken);

            if(currency is null)
                return new ResultDto(400, false, null, "Asset name not found");

            DateTime now = DateTime.UtcNow;

            Asset asset = await _unitOfWork.AssetWriteRepository.CreateAsync(new Asset
            {
                CurrencyId = currencyId,
                UserId = userId,
                CurrentPurchasePrice = currency.PurchasePrice,
                CurrentSalePrice = currency.SalePrice,
                PurchasePrice = currency.PurchasePrice,
                SalePrice = currency.SalePrice,
                Count = (int)intent.Amount,
                IsActive = true,
                PurchaseDate = DateOnly.FromDateTime(now),
                CreatedDate = now,
                UpdatedDate = now
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(200, true, new AssetItemDto(
                asset.Id,
                asset.Count,
                asset.PurchasePrice,
                asset.SalePrice,
                asset.PurchaseDate,
                asset.IsActive,
                new CurrencyRelationDto(
                    currency.Id,
                    currency.Title,
                    currency.SubTitle,
                    currency.PurchasePrice,
                    currency.SalePrice
                )
            ));
        }
    }
}