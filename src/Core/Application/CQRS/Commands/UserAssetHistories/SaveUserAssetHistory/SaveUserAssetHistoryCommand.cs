using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.UserAssetHistories.SaveUserAssetHistory
{
    public record SaveUserAssetHistoryCommand : ICommand
    {

    }

    public class SaveUserAssetHistoryCommandHandler : ICommandHandler<SaveUserAssetHistoryCommand, IBaseResult>
    {
        private readonly IUserTokenService _userTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public SaveUserAssetHistoryCommandHandler(IUserTokenService userTokenService, IUnitOfWork unitOfWork)
        {
            _userTokenService = userTokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(SaveUserAssetHistoryCommand command, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ExecuteWithRetryAsync<IBaseResult>(
                async (uow, ct) =>
                {
                    if (!_userTokenService.IsAuthenticated)
                        return new ResultDto(401, false);

                    int userId = _userTokenService.UserId;
                    int statusCode = 200;

                    using (var tx = uow.BeginTransaction())
                    {
                        try
                        {
                            DateTime now = DateTime.UtcNow;
                            DateOnly doNow = DateOnly.FromDateTime(now);

                            var rule = await uow.UserAssetHistoryRule.CheckValidByGivenDate(userId, doNow, ct);

                            var toCreateItems = await uow
                                .AssetReadRepository
                                .Table
                                .AsNoTracking()
                                .Include(x => x.Currency)
                                .Where(x => x.UserId == userId)
                                .GroupBy(x => x.CurrencyId)
                                .Select(x => new UserAssetItemHistory
                                {
                                    CurrencyId = x.Key,
                                    Count = x.Sum(a => a.Count),
                                    Date = doNow,
                                    CreatedDate = now,
                                    UpdatedDate = now,
                                    IsActive = true,
                                    TotalCostPurchasePrice = x.Sum(a => a.PurchasePrice * a.Count),
                                    ItemAvgCostPurchasePrice = x.Average(a => a.PurchasePrice),
                                    TotalCostSalePrice = x.Sum(a => a.SalePrice * a.Count),
                                    ItemAvgCostSalePrice = x.Average(a => a.SalePrice),
                                    TotalCurrentPurchasePrice = x.Sum(a => a.CurrentPurchasePrice * a.Count),
                                    ItemAvgCurrentPurchasePrice = x.Average(a => a.CurrentPurchasePrice),
                                    TotalCurrentSalePrice = x.Sum(a => a.CurrentSalePrice * a.Count),
                                    ItemAvgCurrentSalePrice = x.Average(a => a.CurrentSalePrice),
                                    TotalPurchasePrice = x.Sum(a => a.Currency.PurchasePrice * a.Count),
                                    ItemAvgPurchasePrice = x.First().Currency.PurchasePrice,
                                    TotalSalePrice = x.Sum(a => a.Currency.SalePrice * a.Count),
                                    ItemAvgSalePrice = x.First().Currency.SalePrice,
                                })
                                .ToListAsync(ct);

                            if (rule.Success)
                            {
                                var toCreateUserHistory = new UserAssetHistory
                                {
                                    UserId = userId,
                                    TotalPurchasePrice = toCreateItems.Sum(i => i.TotalPurchasePrice),
                                    TotalSalePrice = toCreateItems.Sum(i => i.TotalSalePrice),
                                    TotalCostPurchasePrice = toCreateItems.Sum(i => i.TotalCostPurchasePrice),
                                    TotalCostSalePrice = toCreateItems.Sum(i => i.TotalCostSalePrice),
                                    TotalCurrentPurchasePrice = toCreateItems.Sum(i => i.TotalCurrentPurchasePrice),
                                    TotalCurrentSalePrice = toCreateItems.Sum(i => i.TotalCurrentSalePrice),
                                    CreatedDate = now,
                                    UpdatedDate = now,
                                    IsActive = true,
                                    UserAssetItemHistories = toCreateItems,
                                    Date = doNow
                                };

                                await uow.UserAssetHistoryWriteRepository.CreateAsync(toCreateUserHistory, ct);

                                statusCode = 201;
                            }
                            else
                            {
                                var userAssetHistory = await uow.UserAssetHistoryReadRepository
                                    .Table
                                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Date == doNow, ct);

                                if (userAssetHistory == null)
                                {
                                    await tx.RollbackAsync(ct);
                                    return new ResultDto(400, false, null, "Hesap Ozeti Bulunamadi!");
                                }


                                await uow.UserAssetItemHistoryWriteRepository
                                        .Table
                                        .Where(x => x.UserAssetHistoryId == userAssetHistory.Id)
                                        .ExecuteDeleteAsync(ct);

                                foreach (var item in toCreateItems)
                                {
                                    item.UserAssetHistoryId = userAssetHistory.Id;
                                }

                                userAssetHistory.TotalPurchasePrice = toCreateItems.Sum(i => i.TotalPurchasePrice);
                                userAssetHistory.TotalSalePrice = toCreateItems.Sum(i => i.TotalSalePrice);
                                userAssetHistory.TotalCostPurchasePrice = toCreateItems.Sum(i => i.TotalCostPurchasePrice);
                                userAssetHistory.TotalCostSalePrice = toCreateItems.Sum(i => i.TotalCostSalePrice);
                                userAssetHistory.TotalCurrentPurchasePrice = toCreateItems.Sum(i => i.TotalCurrentPurchasePrice);
                                userAssetHistory.TotalCurrentSalePrice = toCreateItems.Sum(i => i.TotalCurrentSalePrice);
                                userAssetHistory.UpdatedDate = DateTime.UtcNow;

                                await uow.UserAssetItemHistoryWriteRepository
                                    .AddRangeAsync(toCreateItems, ct);

                                statusCode = 203;
                            }

                            await uow.SaveChangesAsync(ct);

                            await tx.CommitAsync(ct);

                            return new ResultDto(statusCode, true);
                        }
                        catch (System.Exception ex)
                        {
                            await tx.RollbackAsync(ct);
                            return new ResultDto(500, false);
                        }
                    }
                },
                cancellationToken
            );
        }
    }
}