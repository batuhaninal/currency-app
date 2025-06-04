using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using FluentValidation;

namespace Application.CQRS.Commands.Assets.Update
{
    public sealed record UpdateAssetCommand : ICommand
    {
        public UpdateAssetCommand()
        {

        }

        public UpdateAssetCommand(int count, decimal purchasePrice, decimal salePrice, DateOnly purchaseDate)
        {
            Count = count;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            PurchaseDate = purchaseDate;
        }

        [JsonIgnore]
        public int AssetId { get; set; }
        public int Count { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public DateOnly PurchaseDate { get; init; }
    }

    public sealed class UpdateAssetCommandValidator : AbstractValidator<UpdateAssetCommand>
    {
        public UpdateAssetCommandValidator()
        {
            this.RuleFor(x => x.AssetId)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .GreaterThan(0)
                    .WithMessage(ErrorMessage.Validation.GreaterThan());

            this.RuleFor(x => x.Count)
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