using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Domain;
using FluentValidation;

namespace Application.CQRS.Commands.Categories.Add
{
    public sealed record AddCategoryCommand : ICommand
    {
        public AddCategoryCommand()
        {

        }
        public AddCategoryCommand(string title, bool isActive)
        {
            Title = title.TrimStart().TrimEnd();
            IsActive = isActive;
        }

        public string Title { get; init; } = null!;
        public bool IsActive { get; set; }

        internal Category ToDomain()
        {
            DateTime now = DateTime.UtcNow;
            return new Category
            {
                Title = this.Title,
                IsActive = this.IsActive,
                CreatedDate = now,
                UpdatedDate = now
            };
        }
    }

    public sealed class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {
            this.RuleFor(x => x.Title)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(2)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(50)
                    .WithMessage(ErrorMessage.Validation.MaxLength());
        }
    }
}