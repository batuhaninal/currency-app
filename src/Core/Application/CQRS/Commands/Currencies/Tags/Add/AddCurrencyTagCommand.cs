using System.Text.Json.Serialization;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;
using Domain.Entities;
using FluentValidation;

namespace Application.CQRS.Commands.Currencies.Tags.Add
{
    public sealed class AddCurrencyTagCommand : ICommand
    {
        public AddCurrencyTagCommand()
        {
            
        }
        public AddCurrencyTagCommand(int currencyId,string value)
        {
            CurrencyId = currencyId;
            Value = value;
        }

        public int CurrencyId { get; init; }
        public string Value { get; init; } = null!;

        internal CurrencyTag ToDomain() => new CurrencyTag
        {
            CurrencyId = this.CurrencyId,
            Value = this.Value.TrimStart().TrimEnd().ToLower(),
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            IsActive = true   
        };
    }

    public sealed class AddCurrencyTagCommandValidator : AbstractValidator<AddCurrencyTagCommand>
    {
        public AddCurrencyTagCommandValidator()
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

    public sealed class AddCurrencyTagCommandHandler : ICommandHandler<AddCurrencyTagCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public AddCurrencyTagCommandHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(AddCurrencyTagCommand command, CancellationToken cancellationToken = default)
        {
            var bs = await _unitOfWork.CurrencyTagRule.CheckValueValidAsync(command.Value, cancellationToken);

            if(!bs.Success)
                return bs;
                
            await _unitOfWork.CurrencyTagWriteRepository.CreateAsync(command.ToDomain(), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(200, true);
        }
    }
}