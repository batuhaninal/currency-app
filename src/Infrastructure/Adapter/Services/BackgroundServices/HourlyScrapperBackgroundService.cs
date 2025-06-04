using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.CurrencyHistories;
using Application.Models.DTOs.Externals;
using Domain;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Adapter.Services.BackgroundServices
{
    public sealed class HourlyScrapperBackgroundService : BackgroundService
    {
        private readonly IWebScrappingService _webScrappingService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HourlyScrapperBackgroundService> _logger;

        public HourlyScrapperBackgroundService(IWebScrappingService webScrappingService, IServiceProvider serviceProvider, ILogger<HourlyScrapperBackgroundService> logger)
        {
            _webScrappingService = webScrappingService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
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

                    _logger.LogInformation("{Service} background service execute started at: {Now}", typeof(HourlyScrapperBackgroundService).Name, now);

                    var keys = await unitOfWork
                        .CurrencyReadRepository
                        .Table
                        .AsNoTracking()
                        .Where(x => x.XPath != null && x.XPath != string.Empty)
                        .Select(x => x.XPath)
                        .ToArrayAsync(cancellationToken);

                    _logger.LogInformation("{Service} background service external service request started at: {Now}", typeof(HourlyScrapperBackgroundService).Name, DateTime.UtcNow);

                    var fetchedData = await _webScrappingService.FetchDovizcomXAUDataAsync(keys, cancellationToken);

                    _logger.LogInformation("{Service} background service external service request finished at: {Now}", typeof(HourlyScrapperBackgroundService).Name, DateTime.UtcNow);

                    foreach (var data in fetchedData)
                    {
                        if (data != null && data.Value > 0)
                        {
                            await UpdateValue(unitOfWork, data, now, cancellationToken);
                            await Task.Delay(1_000, cancellationToken);
                        }
                    }

                    await Task.Delay(1_000 * 60 * 60, cancellationToken);
                }
            }
        }

        private async Task UpdateValue(IUnitOfWork unitOfWork, XAUData xauData, DateTime fetchDate, CancellationToken cancellationToken)
        {
            using (var tx = unitOfWork.BeginTransaction())
            {
                try
                {
                    Currency? currency = await unitOfWork
                                .CurrencyReadRepository
                                .Table
                                .FirstOrDefaultAsync(x => x.XPath == xauData.Key, cancellationToken);

                    if (currency == null)
                        return;

                    currency.UpdatedDate = fetchDate;
                    if (xauData.Attr == "bid")
                    {
                        currency.PurchasePrice = xauData.Value;
                    }
                    else if (xauData.Attr == "ask")
                    {
                        currency.SalePrice = xauData.Value;
                    }

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    IBaseResult historyResult = await unitOfWork.CurrencyHistoryRule.CheckCurrencyCountValidAsync(currency.Id, DateOnly.FromDateTime(currency.UpdatedDate), cancellationToken: cancellationToken);
                    if (historyResult.Success)
                    {
                        CurrencyHistoryPriceDto? currencyHistoryPriceDto = await unitOfWork.CurrencyHistoryReadRepository
                            .Table
                            .AsNoTracking()
                            .Where(x => x.CurrencyId == currency.Id)
                            .OrderByDescending(x => x.UpdatedDate)
                            .Select(x => new CurrencyHistoryPriceDto(x.Id, x.CurrencyId, x.OldPurchasePrice, x.NewPurchasePrice, x.OldPurchasePrice, x.NewSalePrice))
                            .FirstOrDefaultAsync(cancellationToken);

                        historyResult = await unitOfWork.CurrencyHistoryRule.CheckCurrencyTimeAsync(currency.Id, currency.UpdatedDate.Hour, cancellationToken);
                        if (historyResult.Success)
                        {
                            CurrencyHistory currencyHistory = (await unitOfWork.CurrencyHistoryReadRepository.FindByConditionAsync(x => x.CurrencyId == currency.Id && x.UpdatedDate.Hour == currency.UpdatedDate.Hour, true, cancellationToken))!;

                            currencyHistory.UpdatedDate = currency.UpdatedDate;
                            currencyHistory.OldPurchasePrice = currencyHistory.NewPurchasePrice;
                            currencyHistory.NewPurchasePrice = currency.PurchasePrice;
                            currencyHistory.OldSalePrice = currencyHistory.NewSalePrice;
                            currencyHistory.NewSalePrice = currency.SalePrice;

                            await unitOfWork.SaveChangesAsync(cancellationToken);
                        }
                        else
                        {
                            _ = await unitOfWork.CurrencyHistoryWriteRepository.CreateAsync(new CurrencyHistory
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

                            await unitOfWork.SaveChangesAsync(cancellationToken);
                        }
                    }

                    await tx.CommitAsync(cancellationToken);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("{Function} exception: {Exception}", typeof(HourlyScrapperBackgroundService).Name, ex);
                    await tx.RollbackAsync(cancellationToken);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }

    public class AltinVerisi
    {
        public string Key { get; set; }
        public string Attr { get; set; }
        public string Value { get; set; }
    }
}