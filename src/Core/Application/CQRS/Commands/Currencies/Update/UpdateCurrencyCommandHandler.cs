using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Currencies.Update
{
    public sealed class UpdateCurrencyCommandHandler : ICommandHandler<UpdateCurrencyCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public UpdateCurrencyCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(UpdateCurrencyCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(command.CurrencyId, cancellationToken);

            if (!result.Success)
                return result;

            result = await _unitOfWork.CategoryRule.CheckExistAsync(command.CategoryId);
            
            if (!result.Success)
                return result;

            result = await _unitOfWork.CurrencyRule.CheckTitleValidAsync(command.CurrencyId, command.Title, cancellationToken);

            if (result.Success)
            {
                Currency currency = (await _unitOfWork.CurrencyReadRepository.GetByIdAsync(command.CurrencyId, true, cancellationToken))!;

                currency.CategoryId = command.CategoryId;
                currency.Title = command.Title;
                currency.SubTitle = command.SubTitle;
                currency.TVCode = command.TvCode;
                currency.IsActive = command.IsActive;
                currency.XPath = command.XPath;

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CurrencyPrefix);

                CategoryRelationDto? category = await _unitOfWork
                    .CategoryReadRepository
                    .Table
                    .AsNoTracking()
                    .Where(x => x.Id == command.CategoryId)
                    .Select(x => new CategoryRelationDto(x.Id, x.Title))
                    .FirstOrDefaultAsync(cancellationToken);

                result = new ResultDto(203, true, new CurrencyInfoDto(
                    currency.Id,
                    currency.Title,
                    currency.SubTitle,
                    currency.TVCode,
                    currency.XPath,
                    currency.PurchasePrice,
                    currency.SalePrice,
                    currency.CreatedDate,
                    currency.UpdatedDate,
                    currency.IsActive,
                    category
                ));
            }

            return result;
        }
    }
}