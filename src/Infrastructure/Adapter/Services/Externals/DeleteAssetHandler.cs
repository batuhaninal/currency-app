using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Services.Externals;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.AIs;
using Application.Models.DTOs.Commons.Results;
using Application.Models.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Services.Externals
{
    public sealed class DeleteAssetHandler : IAITagHandler
    {
        public AiAction AiAction => AiAction.RemoveAsset;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAssetHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> HandleAsync(AiIntent intent, int userId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ExecuteWithRetryAsync<IBaseResult>(
                async (uow, ct) =>
                {
                    using (var tx = uow.BeginTransaction())
                    {
                        try
                        {
                            if (intent is null)
                                return new ResultDto(400, false, null, "Intent is null");

                            if (string.IsNullOrWhiteSpace(intent.AssetName))
                                return new ResultDto(400, false, null, "Asset Name is required");

                            if (intent.Amount is null || intent.Amount <= 0)
                                return new ResultDto(400, false, null, "Amount must be greater than zero");

                            string normalised = intent.AssetName.TrimStart().TrimEnd().ToLower();

                            int currencyId = await uow.
                                CurrencyTagReadRepository.
                                Table.
                                AsNoTracking().
                                Where(x => x.Value.ToLower() == normalised).
                                Select(x => x.CurrencyId).
                                FirstOrDefaultAsync(ct);

                            if (currencyId == 0)
                                return new ResultDto(400, false, null, "Asset name not found");

                            decimal totalAsset = await uow.
                                AssetReadRepository.
                                Table.
                                AsNoTracking().
                                Where(x => x.CurrencyId == currencyId && x.UserId == userId).
                                SumAsync(x => x.Count, ct);

                            if (intent.Amount > totalAsset)
                                return new ResultDto(400, false, null, "You don't have enough asset");

                            var assets = await uow.
                                AssetReadRepository.
                                Table.
                                Where(x => x.CurrencyId == currencyId && x.UserId == userId).
                                OrderBy(x => x.PurchaseDate).
                                    ThenBy(y => y.CreatedDate).
                                ToListAsync(ct);

                            Asset? asset = assets.FirstOrDefault(x => x.Count == intent.Amount);

                            if (asset is not null)
                            {
                                await uow.AssetWriteRepository.RemoveAsync(asset.Id, ct);

                                await uow.SaveChangesAsync(ct);

                                await tx.CommitAsync(ct);

                                return new ResultDto(200, true);
                            }

                            asset = assets.FirstOrDefault(x => x.Count > intent.Amount);

                            if (asset is not null)
                            {
                                asset.Count -= intent.Amount.Value;

                                uow.AssetWriteRepository.Update(asset);

                                await uow.SaveChangesAsync(ct);

                                await tx.CommitAsync(ct);

                                return new ResultDto(200, true);
                            }

                            decimal totalCount = 0;
                            decimal lastCount = 0;

                            List<Asset> toDeleteAssets = new();

                            foreach (var item in assets)
                            {
                                lastCount = totalCount;
                                totalCount += item.Count;

                                if (totalCount == intent.Amount)
                                {
                                    toDeleteAssets.Add(item);
                                    break;
                                }
                                else if (totalCount > intent.Amount)
                                {
                                    item.Count = item.Count - (intent.Amount.Value - lastCount);

                                    uow.AssetWriteRepository.Update(item);
                                    break;
                                }

                                toDeleteAssets.Add(item);
                            }

                            if (toDeleteAssets.Count > 0)
                                uow.AssetWriteRepository.RemoveRange(toDeleteAssets);

                            await uow.SaveChangesAsync(ct);

                            await tx.CommitAsync(ct);

                            return new ResultDto(200, true);
                        }
                        catch (System.Exception ex)
                        {
                            await tx.RollbackAsync(ct);
                            return new ResultDto(500, false, null, ErrorMessage.INTERNAL);
                        }
                    }
                },
                cancellationToken
            );
        }
    }
}