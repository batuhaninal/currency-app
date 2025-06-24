using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Domain.Entities;
using FluentValidation;

namespace Application.CQRS.Commands.Currencies.Add
{
    public sealed record AddCurrencyCommand : ICommand
    {
        public AddCurrencyCommand()
        {

        }

        public AddCurrencyCommand(string title, string? subTitle, string? tvCode, string? xPath, decimal purchasePrice, decimal salePrice, int categoryId, bool isActive)
        {
            Title = title.TrimStart().TrimEnd();
            SubTitle = subTitle != null ? subTitle.TrimStart().TrimEnd() : null;
            TvCode = tvCode != null ? tvCode.TrimStart().TrimEnd() : null;
            XPath = xPath != null ? xPath.TrimStart().TrimEnd() : null;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CategoryId = categoryId;
            IsActive = isActive;
        }

        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public string? XPath { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public int CategoryId { get; init; }
        public bool IsActive { get; init; }

        internal Currency ToDomain()
        {
            DateTime now = DateTime.UtcNow;

            return new Currency
            {
                Title = this.Title,
                SubTitle = this.SubTitle,
                TVCode = this.TvCode,
                XPath = this.XPath,
                PurchasePrice = this.PurchasePrice,
                SalePrice = this.SalePrice,
                CategoryId = this.CategoryId,
                IsActive = this.IsActive,
                CreatedDate = now,
                UpdatedDate = now,
            };
        }
    }

    public sealed class AddCurrencyCommandValidator : AbstractValidator<AddCurrencyCommand>
    {
        public AddCurrencyCommandValidator()
        {
            this.RuleFor(x => x.Title)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(2)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(50)
                    .WithMessage(ErrorMessage.Validation.MaxLength());

            this.RuleFor(x => x.SubTitle)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(2)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(50)
                    .WithMessage(ErrorMessage.Validation.MaxLength())
                .When(x => !string.IsNullOrEmpty(x.SubTitle));

            this.RuleFor(x => x.TvCode)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(1)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(50)
                    .WithMessage(ErrorMessage.Validation.MaxLength())
                .When(x => !string.IsNullOrEmpty(x.TvCode));

            this.RuleFor(x => x.XPath)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(1)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(50)
                    .WithMessage(ErrorMessage.Validation.MaxLength())
                .When(x => !string.IsNullOrEmpty(x.XPath));

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
                    
            this.RuleFor(x => x.CategoryId)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .GreaterThan(0)
                    .WithMessage(ErrorMessage.Validation.GreaterThan());
        }
    }
}