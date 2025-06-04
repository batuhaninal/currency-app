using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Domain;
using FluentValidation;

namespace Application.CQRS.Commands.Categories.Update
{
    public sealed record UpdateCategoryCommand : ICommand
    {
        public UpdateCategoryCommand()
        {

        }

        public UpdateCategoryCommand(int categoryId, string title, bool isActive)
        {
            CategoryId = categoryId;
            Title = title;
            IsActive = isActive;
        }

        [JsonIgnore]
        public int CategoryId { get; set; }
        public string Title { get; init; } = null!;
        public bool IsActive { get; init; }

        internal void ToUpdate(ref Category category)
        {
            category.Title = Title;
            category.IsActive = IsActive;
            category.UpdatedDate = DateTime.UtcNow;
        }
    }

    public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            this.RuleFor(x => x.CategoryId)
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
        }
    }
}