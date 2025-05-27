using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Currencies.ChangeStatus
{
    public sealed class ChangeCurrencyStatusCommandHandler : ICommandHandler<ChangeCurrencyStatusCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeCurrencyStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(ChangeCurrencyStatusCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(command.CurrencyId, cancellationToken);

            if (result.Success)
            {
                await _unitOfWork
                    .CurrencyWriteRepository
                    .Table
                    .Where(x => x.Id == command.CurrencyId)
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(p => p.IsActive, v => !v.IsActive),
                        cancellationToken
                    );

                result = new ResultDto(203, true);
            }

            return result;
        }
    }
}