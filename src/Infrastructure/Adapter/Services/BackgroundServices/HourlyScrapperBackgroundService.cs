using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Logger;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Notifiers;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Services.Externals;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.CurrencyHistories;
using Application.Models.DTOs.Externals;
using Application.Models.Events;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adapter.Services.BackgroundServices
{
    public sealed class HourlyScrapperBackgroundService : BackgroundService
    {
        private readonly IWebScrappingService _webScrappingService;
        private readonly IServiceProvider _serviceProvider;
        // private readonly ILogger<HourlyScrapperBackgroundService> _logger;
        private readonly ICurrencyNotifierService _currencyNotifierService;
        private readonly ILoggerService<HourlyScrapperBackgroundService> _logger;
        private readonly ICacheService _cacheService;

        public HourlyScrapperBackgroundService(IWebScrappingService webScrappingService, IServiceProvider serviceProvider, ILoggerService<HourlyScrapperBackgroundService> logger, ICurrencyNotifierService currencyNotifierService, ICacheService cacheService)
        {
            _webScrappingService = webScrappingService;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _currencyNotifierService = currencyNotifierService;
            _cacheService = cacheService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("background service started");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    DateTime now = DateTime.UtcNow;

                    _logger.Info("background service execute started");

                    var keys = await unitOfWork
                        .CurrencyReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(x => x.XPath != null && x.XPath != string.Empty)
                        .Select(x => x.XPath)
                        .ToArrayAsync(cancellationToken);

                    _logger.Info("background service request started");

                    var fetchedData = await _webScrappingService.FetchDovizcomXAUDataAsync(keys, cancellationToken);

                    _logger.Info("background service request finished");

                    foreach (var data in fetchedData)
                    {
                        if (data != null && data.Value > 0)
                        {
                            await UpdateValue(unitOfWork, data, now, cancellationToken);
                            await Task.Delay(1_000, cancellationToken);
                        }
                    }

                    await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CurrencyPrefix);

                    await Task.Delay(1_000 * 60 * 60, cancellationToken);
                    // await Task.Delay(1_000 * 60 * 1, cancellationToken);
                }
            }
        }

        private async Task UpdateValue(IUnitOfWork unitOfWork, XAUData xauData, DateTime fetchDate, CancellationToken cancellationToken)
        {
            await unitOfWork.ExecuteWithRetryAsync(
                async (uow, ct) =>
                {
                    using (var tx = uow.BeginTransaction())
                    {
                        try
                        {
                            Currency? currency = await uow
                                        .CurrencyReadRepository
                                        .Table
                                        .FirstOrDefaultAsync(x => x.XPath == xauData.Key, cancellationToken);

                            if (currency == null)
                                return true;

                            currency.UpdatedDate = fetchDate;
                            if (xauData.Attr == "bid")
                            {
                                currency.PurchasePrice = xauData.Value;
                            }
                            else if (xauData.Attr == "ask")
                            {
                                currency.SalePrice = xauData.Value;
                            }

                            await uow.SaveChangesAsync(cancellationToken);

                            IBaseResult historyResult = await uow.CurrencyHistoryRule.CheckCurrencyCountValidAsync(currency.Id, DateOnly.FromDateTime(currency.UpdatedDate), cancellationToken: cancellationToken);
                            if (historyResult.Success)
                            {
                                historyResult = await uow.CurrencyHistoryRule.CheckCurrencyTimeAsync(currency.Id, currency.UpdatedDate, cancellationToken);
                                if (historyResult.Success)
                                {
                                    CurrencyHistory currencyHistory = (await uow.CurrencyHistoryReadRepository.FindByConditionAsync(x => x.CurrencyId == currency.Id && x.CreatedDate.Date == currency.UpdatedDate.Date && x.Date == DateOnly.FromDateTime(currency.UpdatedDate.Date) && x.UpdatedDate.Year == currency.UpdatedDate.Year && x.UpdatedDate.Month == currency.UpdatedDate.Month && x.UpdatedDate.Day == currency.UpdatedDate.Day && x.UpdatedDate.Hour == currency.UpdatedDate.Hour, true, cancellationToken))!;

                                    currencyHistory.UpdatedDate = currency.UpdatedDate;
                                    currencyHistory.OldPurchasePrice = currencyHistory.NewPurchasePrice;
                                    currencyHistory.NewPurchasePrice = currency.PurchasePrice;
                                    currencyHistory.OldSalePrice = currencyHistory.NewSalePrice;
                                    currencyHistory.NewSalePrice = currency.SalePrice;

                                    await uow.SaveChangesAsync(cancellationToken);
                                }
                                else
                                {
                                    CurrencyHistoryPriceDto? currencyHistoryPriceDto = await uow.CurrencyHistoryReadRepository
                                        .Table
                                        .AsNoTracking()
                                        .Where(x => x.CurrencyId == currency.Id)
                                        .OrderByDescending(x => x.UpdatedDate)
                                        .Select(x => new CurrencyHistoryPriceDto(x.Id, x.CurrencyId, x.OldPurchasePrice, x.NewPurchasePrice, x.OldPurchasePrice, x.NewSalePrice))
                                        .FirstOrDefaultAsync(cancellationToken);

                                    _ = await uow.CurrencyHistoryWriteRepository.CreateAsync(new CurrencyHistory
                                    {
                                        CurrencyId = currency.Id,
                                        Date = DateOnly.FromDateTime(currency.UpdatedDate),
                                        CreatedDate = currency.CreatedDate,
                                        UpdatedDate = currency.UpdatedDate,
                                        IsActive = true,
                                        NewPurchasePrice = currency.PurchasePrice,
                                        OldPurchasePrice = currencyHistoryPriceDto == null ? currency.PurchasePrice : currencyHistoryPriceDto.NewPurchasePrice,
                                        NewSalePrice = currency.SalePrice,
                                        OldSalePrice = currencyHistoryPriceDto == null ? currency.SalePrice : currencyHistoryPriceDto.NewSalePrice,
                                    }, cancellationToken);

                                    await uow.SaveChangesAsync(cancellationToken);
                                }
                            }

                            await tx.CommitAsync(cancellationToken);

                            await _currencyNotifierService.NotifyPriceAsync(new PriceUpdatedEvent(currency.Id, currency.Title, currency.PurchasePrice, currency.SalePrice), cancellationToken);

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
            //         Currency? currency = await unitOfWork
            //                     .CurrencyReadRepository
            //                     .Table
            //                     .FirstOrDefaultAsync(x => x.XPath == xauData.Key, cancellationToken);

            //         if (currency == null)
            //             return;

            //         currency.UpdatedDate = fetchDate;
            //         if (xauData.Attr == "bid")
            //         {
            //             currency.PurchasePrice = xauData.Value;
            //         }
            //         else if (xauData.Attr == "ask")
            //         {
            //             currency.SalePrice = xauData.Value;
            //         }

            //         await unitOfWork.SaveChangesAsync(cancellationToken);

            //         IBaseResult historyResult = await unitOfWork.CurrencyHistoryRule.CheckCurrencyCountValidAsync(currency.Id, DateOnly.FromDateTime(currency.UpdatedDate), cancellationToken: cancellationToken);
            //         if (historyResult.Success)
            //         {
            //             historyResult = await unitOfWork.CurrencyHistoryRule.CheckCurrencyTimeAsync(currency.Id, currency.UpdatedDate, cancellationToken);
            //             if (historyResult.Success)
            //             {
            //                 CurrencyHistory currencyHistory = (await unitOfWork.CurrencyHistoryReadRepository.FindByConditionAsync(x => x.CurrencyId == currency.Id && x.CreatedDate.Date == currency.UpdatedDate.Date && x.Date == DateOnly.FromDateTime(currency.UpdatedDate.Date) && x.UpdatedDate.Year == currency.UpdatedDate.Year && x.UpdatedDate.Month == currency.UpdatedDate.Month && x.UpdatedDate.Day == currency.UpdatedDate.Day && x.UpdatedDate.Hour == currency.UpdatedDate.Hour, true, cancellationToken))!;

            //                 currencyHistory.UpdatedDate = currency.UpdatedDate;
            //                 currencyHistory.OldPurchasePrice = currencyHistory.NewPurchasePrice;
            //                 currencyHistory.NewPurchasePrice = currency.PurchasePrice;
            //                 currencyHistory.OldSalePrice = currencyHistory.NewSalePrice;
            //                 currencyHistory.NewSalePrice = currency.SalePrice;

            //                 await unitOfWork.SaveChangesAsync(cancellationToken);
            //             }
            //             else
            //             {
            //                 CurrencyHistoryPriceDto? currencyHistoryPriceDto = await unitOfWork.CurrencyHistoryReadRepository
            //                     .Table
            //                     .AsNoTracking()
            //                     .Where(x => x.CurrencyId == currency.Id)
            //                     .OrderByDescending(x => x.UpdatedDate)
            //                     .Select(x => new CurrencyHistoryPriceDto(x.Id, x.CurrencyId, x.OldPurchasePrice, x.NewPurchasePrice, x.OldPurchasePrice, x.NewSalePrice))
            //                     .FirstOrDefaultAsync(cancellationToken);

            //                 _ = await unitOfWork.CurrencyHistoryWriteRepository.CreateAsync(new CurrencyHistory
            //                 {
            //                     CurrencyId = currency.Id,
            //                     Date = DateOnly.FromDateTime(currency.UpdatedDate),
            //                     CreatedDate = currency.CreatedDate,
            //                     UpdatedDate = currency.UpdatedDate,
            //                     IsActive = true,
            //                     NewPurchasePrice = currency.PurchasePrice,
            //                     OldPurchasePrice = currencyHistoryPriceDto == null ? currency.PurchasePrice : currencyHistoryPriceDto.NewPurchasePrice,
            //                     NewSalePrice = currency.SalePrice,
            //                     OldSalePrice = currencyHistoryPriceDto == null ? currency.SalePrice : currencyHistoryPriceDto.NewSalePrice,
            //                 }, cancellationToken);

            //                 await unitOfWork.SaveChangesAsync(cancellationToken);
            //             }
            //         }

            //         await tx.CommitAsync(cancellationToken);
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