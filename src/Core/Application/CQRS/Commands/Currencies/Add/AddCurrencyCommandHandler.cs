using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Commands.Currencies.Add
{
    public sealed class AddCurrencyCommandHandler : ICommandHandler<AddCurrencyCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddCurrencyCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public AddCurrencyCommandHandler(IUnitOfWork unitOfWork, ILogger<AddCurrencyCommandHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(AddCurrencyCommand command, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ExecuteWithRetryAsync<IBaseResult>(
                async (uow, ct) =>
                {
                    using (var tx = uow.BeginTransaction())
                    {
                        try
                        {
                            IBaseResult result = await uow.CurrencyRule.CheckTitleValidAsync(command.Title, ct);

                            if (result.Success)
                            {
                                Currency currency = await uow.CurrencyWriteRepository.CreateAsync(command.ToDomain(), ct);

                                await uow.SaveChangesAsync(ct);

                                IBaseResult historyResult = await uow.CurrencyHistoryRule.CheckCurrencyCountValidAsync(currency.Id, DateOnly.FromDateTime(currency.UpdatedDate), cancellationToken: ct);
                                if (historyResult.Success)
                                {
                                    CurrencyHistoryPriceDto? currencyHistoryPriceDto = await uow.CurrencyHistoryReadRepository
                                        .Table
                                        .AsNoTracking()
                                        .Where(x => x.CurrencyId == currency.Id)
                                        .OrderByDescending(x => x.UpdatedDate)
                                        .Select(x => new CurrencyHistoryPriceDto(x.Id, x.CurrencyId, x.OldPurchasePrice, x.NewPurchasePrice, x.OldPurchasePrice, x.NewSalePrice))
                                        .FirstOrDefaultAsync(ct);

                                    historyResult = await uow.CurrencyHistoryRule.CheckCurrencyTimeAsync(currency.Id, currency.UpdatedDate, ct);
                                    if (historyResult.Success)
                                    {
                                        CurrencyHistory currencyHistory = (await uow.CurrencyHistoryReadRepository.FindByConditionAsync(x => x.CurrencyId == currency.Id && x.CreatedDate.Date == currency.UpdatedDate.Date && x.Date == DateOnly.FromDateTime(currency.UpdatedDate.Date) && x.UpdatedDate.Year == currency.UpdatedDate.Year && x.UpdatedDate.Month == currency.UpdatedDate.Month && x.UpdatedDate.Day == currency.UpdatedDate.Day && x.UpdatedDate.Hour == currency.UpdatedDate.Hour, true, ct))!;

                                        currencyHistory.UpdatedDate = currency.UpdatedDate;
                                        currencyHistory.OldPurchasePrice = currencyHistory.NewPurchasePrice;
                                        currencyHistory.NewPurchasePrice = currency.PurchasePrice;
                                        currencyHistory.OldSalePrice = currencyHistory.NewSalePrice;
                                        currencyHistory.NewSalePrice = currency.SalePrice;

                                        await uow.SaveChangesAsync(ct);
                                    }
                                    else
                                    {
                                        await uow.CurrencyHistoryWriteRepository.CreateAsync(new CurrencyHistory
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
                                        }, ct);

                                        await uow.SaveChangesAsync(ct);
                                    }
                                }
                                await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CurrencyPrefix);
                                CategoryRelationDto? category = await uow.CategoryReadRepository.Table.AsNoTracking().Where(x => x.Id == currency.CategoryId).Select(x => new CategoryRelationDto(x.Id, x.Title)).FirstOrDefaultAsync();
                                result = new ResultDto(201, true, new CurrencyItemDto(currency.Id, currency.Title, currency.SubTitle, currency.TVCode, currency.XPath, currency.PurchasePrice, currency.SalePrice, currency.IsActive, category));
                            }

                            await tx.CommitAsync(ct);

                            return result;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError("{Function} exception: {Exception}", typeof(AddCurrencyCommandHandler).Name, ex);
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