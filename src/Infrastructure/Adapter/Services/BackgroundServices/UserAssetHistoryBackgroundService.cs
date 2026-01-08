using Application.Abstractions.Commons.Logger;
using Application.Abstractions.Repositories.Commons;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adapter.Services.BackgroundServices
{
    public sealed class UserAssetHistoryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerService<UserAssetHistoryBackgroundService> _logger;

        public UserAssetHistoryBackgroundService(IServiceProvider serviceProvider, ILoggerService<UserAssetHistoryBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("background service starting");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1_000 * 30, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    _logger.Info("background service execute starting");

                    if (DateTime.UtcNow.Hour >= 15)
                    {
                        var userIds = await unitOfWork
                            .UserReadRepository
                            .Table
                            .AsNoTracking()
                            .Include(x => x.Assets)
                            .Where(x => x.Assets != null && x.Assets.Any())
                            .Select(x => x.Id)
                            .ToArrayAsync();

                        foreach (var item in userIds)
                        {
                            await SaveUserAssetHistories(unitOfWork, item, stoppingToken);
                            await Task.Delay(1_000, stoppingToken);
                        }

                        _logger.Info($"background service execution finished succesfully now going to wait at: {DateTime.Now.AddHours(10)}");
                        await Task.Delay(1_000 * 60 * 60 * 10, stoppingToken);
                        continue;
                    }

                    _logger.Info("background service execute finished");
                    await Task.Delay(1_000 * 60 * 60 * 1, stoppingToken);
                }
            }
        }

        private async Task SaveUserAssetHistories(IUnitOfWork unitOfWork, int userId, CancellationToken cancellationToken)
        {
            // Retry mekanizmasi ile manuel transaction kullanmak mumkun degil
            await unitOfWork.ExecuteWithRetryAsync(
                async (uow, ct) =>
                {
                    using (var tx = uow.BeginTransaction())
                    {
                        try
                        {
                            DateTime now = DateTime.UtcNow;
                            DateOnly doNow = DateOnly.FromDateTime(now);

                            var rule = await uow.UserAssetHistoryRule.CheckValidByGivenDate(userId, doNow, cancellationToken);
                            if (rule.Success)
                            {
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
                                .ToListAsync(cancellationToken);

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

                                await uow.UserAssetHistoryWriteRepository.CreateAsync(toCreateUserHistory, cancellationToken);

                                await uow.SaveChangesAsync(cancellationToken);

                                await tx.CommitAsync(cancellationToken);
                            }
                            return true;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.Error(ex.Message);
                            await tx.RollbackAsync(cancellationToken);
                            return false;
                        }
                    }
                },
                cancellationToken
            );
            // using (var tx = unitOfWork.BeginTransaction())
            // {
            //     try
            //     {
            //         DateTime now = DateTime.UtcNow;
            //         DateOnly doNow = DateOnly.FromDateTime(now);

            //         var rule = await unitOfWork.UserAssetHistoryRule.CheckValidByGivenDate(userId, doNow, cancellationToken);
            //         if (rule.Success)
            //         {
            //             var toCreateItems = await unitOfWork
            //             .AssetReadRepository
            //             .Table
            //             .AsNoTracking()
            //             .Include(x => x.Currency)
            //             .Where(x => x.UserId == userId)
            //             .GroupBy(x => x.CurrencyId)
            //             .Select(x => new UserAssetItemHistory
            //             {
            //                 CurrencyId = x.Key,
            //                 Count = x.Sum(a => a.Count),
            //                 Date = doNow,
            //                 CreatedDate = now,
            //                 UpdatedDate = now,
            //                 IsActive = true,
            //                 TotalCostPurchasePrice = x.Sum(a => a.PurchasePrice * a.Count),
            //                 ItemAvgCostPurchasePrice = x.Average(a => a.PurchasePrice),
            //                 TotalCostSalePrice = x.Sum(a => a.SalePrice * a.Count),
            //                 ItemAvgCostSalePrice = x.Average(a => a.SalePrice),
            //                 TotalCurrentPurchasePrice = x.Sum(a => a.CurrentPurchasePrice * a.Count),
            //                 ItemAvgCurrentPurchasePrice = x.Average(a => a.CurrentPurchasePrice),
            //                 TotalCurrentSalePrice = x.Sum(a => a.CurrentSalePrice * a.Count),
            //                 ItemAvgCurrentSalePrice = x.Average(a => a.CurrentSalePrice),
            //                 TotalPurchasePrice = x.Sum(a => a.Currency.PurchasePrice * a.Count),
            //                 ItemAvgPurchasePrice = x.First().Currency.PurchasePrice,
            //                 TotalSalePrice = x.Sum(a => a.Currency.SalePrice * a.Count),
            //                 ItemAvgSalePrice = x.First().Currency.SalePrice,
            //             })
            //             .ToListAsync(cancellationToken);

            //             var toCreateUserHistory = new UserAssetHistory
            //             {
            //                 UserId = userId,
            //                 TotalPurchasePrice = toCreateItems.Sum(i => i.TotalPurchasePrice),
            //                 TotalSalePrice = toCreateItems.Sum(i => i.TotalSalePrice),
            //                 TotalCostPurchasePrice = toCreateItems.Sum(i => i.TotalCostPurchasePrice),
            //                 TotalCostSalePrice = toCreateItems.Sum(i => i.TotalCostSalePrice),
            //                 TotalCurrentPurchasePrice = toCreateItems.Sum(i => i.TotalCurrentPurchasePrice),
            //                 TotalCurrentSalePrice = toCreateItems.Sum(i => i.TotalCurrentSalePrice),
            //                 CreatedDate = now,
            //                 UpdatedDate = now,
            //                 IsActive = true,
            //                 UserAssetItemHistories = toCreateItems,
            //                 Date = doNow
            //             };

            //             await unitOfWork.UserAssetHistoryWriteRepository.CreateAsync(toCreateUserHistory, cancellationToken);

            //             await unitOfWork.SaveChangesAsync(cancellationToken);

            //             await tx.CommitAsync(cancellationToken);
            //         }
            //     }
            //     catch (System.Exception ex)
            //     {
            //         _logger.Error(ex.Message);
            //         await tx.RollbackAsync(cancellationToken);
            //     }
            // }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("background service stopped");
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.Info("background service disposed");
            base.Dispose();
        }
    }
}