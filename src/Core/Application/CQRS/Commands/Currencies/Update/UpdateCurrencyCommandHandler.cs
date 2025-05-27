using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.Currencies;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Currencies.Update
{
    public sealed class UpdateCurrencyCommandHandler : ICommandHandler<UpdateCurrencyCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCurrencyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                await _unitOfWork.SaveChangesAsync(cancellationToken);

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