using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.CurrencyHistories;
using Application.Models.DTOs.Externals;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Adapter.Services.BackgroundServices
{
    public sealed class HourlyCurrencyBackgroundService : BackgroundService
    {
        private readonly ITradingViewService _tradingViewService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HourlyCurrencyBackgroundService> _logger;

        public HourlyCurrencyBackgroundService(ITradingViewService tradingViewService, IServiceProvider serviceProvider, ILogger<HourlyCurrencyBackgroundService> logger)
        {
            _tradingViewService = tradingViewService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Service} background service started at: {Now}", typeof(HourlyCurrencyBackgroundService).Name, DateTime.UtcNow);
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

                    _logger.LogInformation("{Service} background service execute started at: {Now}", typeof(HourlyCurrencyBackgroundService).Name, now);

                    List<Currency> currencies = await unitOfWork
                        .CurrencyReadRepository
                        .Table
                        .AsNoTracking()
                        .ToListAsync(stoppingToken);

                    string[] currencyList = currencies.Select(x => x.TVCode ?? "RENALDOMESSI").ToArray();

                    _logger.LogInformation("{Service} background service external service request started at: {Now}", typeof(HourlyCurrencyBackgroundService).Name, now);

                    var tradingData = await _tradingViewService.FetchData(1_000, "TRY", cancellationToken: stoppingToken, currencyList);

                    _logger.LogInformation("{Service} background service external service request finished at: {Now}", typeof(HourlyCurrencyBackgroundService).Name, now);

                    foreach (var item in tradingData)
                    {
                        Console.WriteLine(item.TotalCount);
                        Console.WriteLine(item.Data);
                    }

                    foreach (var currency in currencies)
                    {
                        if (currency.TVCode != null)
                        {
                            TradingViewResponseData? trading = tradingData.SelectMany(x => x.Data).FirstOrDefault(a => a.Currency == currency.TVCode);
                            if (trading != null)
                            {
                                await UpdateValue(unitOfWork, currency, trading, stoppingToken);
                                await Task.Delay(1_000, stoppingToken);
                            }
                        }
                    }

                    await Task.Delay(1_000 * 60 * 60, stoppingToken);
                }
            }
        }

        private async Task UpdateValue(IUnitOfWork unitOfWork, Currency currency, TradingViewResponseData tradingViewResponse, CancellationToken cancellationToken)
        {
            using (var tx = unitOfWork.BeginTransaction())
            {
                try
                {
                    currency.UpdatedDate = tradingViewResponse.Date.Value;
                    currency.PurchasePrice = tradingViewResponse.D[0];
                    currency.SalePrice = tradingViewResponse.D[0];

                    unitOfWork.CurrencyWriteRepository.Update(currency);

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    IBaseResult historyResult = await unitOfWork.CurrencyHistoryRule.CheckCurrencyCountAsync(currency.Id, DateOnly.FromDateTime(currency.UpdatedDate), cancellationToken: cancellationToken);
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
                            await unitOfWork.CurrencyHistoryWriteRepository.CreateAsync(new CurrencyHistory
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
                    _logger.LogError("{Function} exception: {Exception}", typeof(HourlyCurrencyBackgroundService).Name, ex);
                    await tx.RollbackAsync(cancellationToken);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Service} background service stopped at: {Now}", typeof(HourlyCurrencyBackgroundService).Name, DateTime.UtcNow);
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation("{Service} background service disposed at: {Now}", typeof(HourlyCurrencyBackgroundService).Name, DateTime.UtcNow);
            base.Dispose();
        }
    }
}