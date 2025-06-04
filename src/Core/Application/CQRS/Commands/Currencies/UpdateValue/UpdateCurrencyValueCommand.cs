using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using FluentValidation;

namespace Application.CQRS.Commands.Currencies.UpdateValue
{
    public sealed record UpdateCurrencyValueCommand : ICommand
    {
        public UpdateCurrencyValueCommand()
        {

        }

        public UpdateCurrencyValueCommand(int currencyId, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        [JsonIgnore]
        public int CurrencyId { get; set; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }

    public sealed class UpdateCurrencyValueCommandValidator : AbstractValidator<UpdateCurrencyValueCommand>
    {
        public UpdateCurrencyValueCommandValidator()
        {
            this.RuleFor(x => x.CurrencyId)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .GreaterThan(0)
                    .WithMessage(ErrorMessage.Validation.GreaterThan());

            this.RuleFor(x => x.PurchasePrice)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .GreaterThan(0)
                    .WithMessage(ErrorMessage.Validation.GreaterThan());

            this.RuleFor(x => x.SalePrice)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .GreaterThan(0)
                    .WithMessage(ErrorMessage.Validation.GreaterThan());      
        }
    }
}