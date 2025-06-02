using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.CurrencyHistories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Commands.Currencies.UpdateValue
{
    public sealed class UpdateCurrencyValueCommandHandler : ICommandHandler<UpdateCurrencyValueCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCurrencyValueCommandHandler> _logger;

        public UpdateCurrencyValueCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCurrencyValueCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IBaseResult> Handle(UpdateCurrencyValueCommand command, CancellationToken cancellationToken = default)
        {
            using (var tx = _unitOfWork.BeginTransaction())
            {
                try
                {
                    IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(command.CurrencyId, cancellationToken);

                    if (result.Success)
                    {
                        Currency currency = (await _unitOfWork.CurrencyReadRepository.GetByIdAsync(command.CurrencyId, true, cancellationToken))!;

                        DateTime now = DateTime.UtcNow;

                        currency.UpdatedDate = now;
                        currency.PurchasePrice = command.PurchasePrice;
                        currency.SalePrice = command.SalePrice;

                        await _unitOfWork.SaveChangesAsync(cancellationToken);

                        IBaseResult historyResult = await _unitOfWork.CurrencyHistoryRule.CheckCurrencyCountValidAsync(currency.Id, DateOnly.FromDateTime(currency.UpdatedDate), cancellationToken: cancellationToken);
                        if (historyResult.Success)
                        {
                            CurrencyHistoryPriceDto? currencyHistoryPriceDto = await _unitOfWork.CurrencyHistoryReadRepository
                                .Table
                                .AsNoTracking()
                                .Where(x => x.CurrencyId == currency.Id)
                                .OrderByDescending(x => x.UpdatedDate)
                                .Select(x => new CurrencyHistoryPriceDto(x.Id, x.CurrencyId, x.OldPurchasePrice, x.NewPurchasePrice, x.OldPurchasePrice, x.NewSalePrice))
                                .FirstOrDefaultAsync(cancellationToken);

                            historyResult = await _unitOfWork.CurrencyHistoryRule.CheckCurrencyTimeAsync(currency.Id, currency.UpdatedDate.Hour, cancellationToken);
                            if (historyResult.Success)
                            {
                                CurrencyHistory currencyHistory = (await _unitOfWork.CurrencyHistoryReadRepository.FindByConditionAsync(x => x.CurrencyId == currency.Id && x.UpdatedDate.Hour == currency.UpdatedDate.Hour, true, cancellationToken))!;

                                currencyHistory.UpdatedDate = currency.UpdatedDate;
                                currencyHistory.OldPurchasePrice = currencyHistory.NewPurchasePrice;
                                currencyHistory.NewPurchasePrice = currency.PurchasePrice;
                                currencyHistory.OldSalePrice = currencyHistory.NewSalePrice;
                                currencyHistory.NewSalePrice = currency.SalePrice;

                                await _unitOfWork.SaveChangesAsync(cancellationToken);
                            }
                            else
                            {
                                await _unitOfWork.CurrencyHistoryWriteRepository.CreateAsync(new CurrencyHistory
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

                                await _unitOfWork.SaveChangesAsync(cancellationToken);
                            }
                        }
                        CategoryRelationDto? category = await _unitOfWork.CategoryReadRepository.Table.AsNoTracking().Where(x => x.Id == currency.CategoryId).Select(x => new CategoryRelationDto(x.Id, x.Title)).FirstOrDefaultAsync();
                        result = new ResultDto(200, true, new CurrencyItemDto(currency.Id, currency.Title, currency.SubTitle, currency.TVCode, currency.PurchasePrice, currency.SalePrice, currency.IsActive, category));
                    }
                    
                     await tx.CommitAsync(cancellationToken);

                    return result;
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("{Function} exception: {Exception}", typeof(UpdateCurrencyValueCommand).Name, ex);
                    await tx.RollbackAsync(cancellationToken);
                    return new ResultDto(500, false, null, ErrorMessage.INTERNAL);
                }
            }
        }
    }
}