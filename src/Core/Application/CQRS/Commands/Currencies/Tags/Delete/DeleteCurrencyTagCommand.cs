using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;

namespace Application.CQRS.Commands.Currencies.Tags.Delete
{
    public sealed record DeleteCurrencyTagCommand : ICommand
    {
        public DeleteCurrencyTagCommand()
        {
            
        }

        public DeleteCurrencyTagCommand(int currencyTagId)
        {
            CurrencyTagId = currencyTagId;
        }
        public int CurrencyTagId { get; init; }
    }

    public sealed class DeleteCurrencyTagCommandHandler : ICommandHandler<DeleteCurrencyTagCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCurrencyTagCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(DeleteCurrencyTagCommand command, CancellationToken cancellationToken = default)
        {
            var bs = await _unitOfWork.CurrencyTagRule.CheckExistAsync(command.CurrencyTagId, cancellationToken);

            if(!bs.Success)
                return bs;

            await _unitOfWork.CurrencyTagWriteRepository.RemoveAsync(command.CurrencyTagId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(200, true);  
        }
    }
}