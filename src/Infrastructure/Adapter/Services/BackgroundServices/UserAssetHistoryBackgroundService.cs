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
            _logger.Info("background service started");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    DateTime now = DateTime.UtcNow;

                    _logger.Info("background service execute started");

                    if (DateTime.Now.Hour == 22)
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

                        _logger.Info($"background service execution finished succesfully now going to wait at: {DateTime.UtcNow.AddHours(12)}");
                        await Task.Delay(1_000 * 60 * 12, stoppingToken);
                    }

                    _logger.Info("background service execute finished");
                    await Task.Delay(1_000 * 60 * 10, stoppingToken);
                }
            }
        }

        private async Task SaveUserAssetHistories(IUnitOfWork unitOfWork, int userId, CancellationToken cancellationToken)
        {
            using (var tx = unitOfWork.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.UtcNow;
                    DateOnly doNow = DateOnly.FromDateTime(now);

                    var toCreateItems = await unitOfWork
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

                    await unitOfWork.UserAssetHistoryWriteRepository.CreateAsync(toCreateUserHistory, cancellationToken);

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    await tx.CommitAsync(cancellationToken);
                }
                catch (System.Exception ex)
                {
                    _logger.Error(ex.Message);
                    await tx.RollbackAsync(cancellationToken);
                }
            }
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