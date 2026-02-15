using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;
using Domain.Entities;
using FluentValidation;

namespace Application.CQRS.Commands.Currencies.Tags.Update
{
    public sealed record UpdateCurrencyTagCommand : ICommand
    {
        public UpdateCurrencyTagCommand()
        {
            
        }

        public UpdateCurrencyTagCommand(int currencyId, string value)
        {
            CurrencyId = currencyId;
            Value = value;
        }

        public int CurrencyTagId { get; set; }
        public int CurrencyId { get; init; }
        public string Value { get; init; }
    }

    public sealed class UpdateCurrencyTagCommandValidator : AbstractValidator<UpdateCurrencyTagCommand>
    {
        public UpdateCurrencyTagCommandValidator()
        {
            this.RuleFor(x => x.Value)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(2)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(70)
                    .WithMessage(ErrorMessage.Validation.MaxLength());
        }
    }

    public sealed class UpdateCurrencyTagCommandHandler : ICommandHandler<UpdateCurrencyTagCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public UpdateCurrencyTagCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UpdateCurrencyTagCommand command, CancellationToken cancellationToken = default)
        {
            var bs = await _unitOfWork.CurrencyTagRule.CheckExistAsync(command.CurrencyTagId, cancellationToken);

            if (!bs.Success)
                return bs;

            bs = await _unitOfWork.CurrencyTagRule.CheckValueValidAsync(command.CurrencyTagId, command.Value, cancellationToken);

            if(!bs.Success)
                return bs;

            CurrencyTag? tag = await _unitOfWork.CurrencyTagReadRepository.GetByIdAsync(command.CurrencyTagId, true, cancellationToken);

            tag.CurrencyId = command.CurrencyId;

            tag.Value = command.Value;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new  ResultDto(200, true);
        }
    }
}