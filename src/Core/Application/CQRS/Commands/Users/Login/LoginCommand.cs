using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using FluentValidation;

namespace Application.CQRS.Commands.Users.Login
{
    public sealed record LoginCommand : ICommand
    {
        public LoginCommand()
        {

        }
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
    }

    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            this.RuleFor(x => x.Username)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(5)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(100)
                    .WithMessage(ErrorMessage.Validation.MaxLength())
                .EmailAddress()
                    .WithMessage(ErrorMessage.Validation.Email);

            this.RuleFor(x => x.Password)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(5)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(100)
                    .WithMessage(ErrorMessage.Validation.MaxLength());
        }
    }
}