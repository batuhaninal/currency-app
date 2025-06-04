using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using FluentValidation;

namespace Application.CQRS.Commands.Currencies.Update
{
    public sealed record UpdateCurrencyCommand : ICommand
    {
        public UpdateCurrencyCommand()
        {

        }

        public UpdateCurrencyCommand(int currencyId, int categoryId, string title, string? subTitle, string? tvCode, string? xPath, bool isActive)
        {
            CurrencyId = currencyId;
            CategoryId = categoryId;
            Title = title;
            SubTitle = subTitle != null ? subTitle.TrimStart().TrimEnd() : null;
            TvCode = tvCode != null ? tvCode.TrimStart().TrimEnd() : null;
            XPath = xPath != null ? xPath.TrimStart().TrimEnd() : null;
            IsActive = isActive;
        }

        [JsonIgnore]
        public int CurrencyId { get; set; }
        public int CategoryId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public string? XPath { get; init; }
        public bool IsActive { get; init; }
    }

    public sealed class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
    {
        public UpdateCurrencyCommandValidator()
        {
            this.RuleFor(x => x.CurrencyId)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .GreaterThan(0)
                    .WithMessage(ErrorMessage.Validation.GreaterThan());

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
                    
            this.RuleFor(x => x.CategoryId)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .GreaterThan(0)
                    .WithMessage(ErrorMessage.Validation.GreaterThan());
        }
    }
}