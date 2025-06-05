using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Domain.Entities;
using FluentValidation;

namespace Application.CQRS.Commands.UpdateProfile
{
    public sealed record UpdateProfileCommand : ICommand
    {
        public UpdateProfileCommand()
        {

        }

        public UpdateProfileCommand(string firstName, string lastName, string? oldPassword, string? newPassword, string? repeatPassword)
        {
            FirstName = firstName;
            LastName = lastName;
            OldPassword = oldPassword;
            NewPassword = newPassword;
            RepeatPassword = repeatPassword;
        }

        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string? OldPassword { get; init; }
        public string? NewPassword { get; init; }
        public string? RepeatPassword { get; init; }

        internal void ToDomain(ref User user, byte[]? pswHash = null, byte[]? pswSalt = null)
        {
            user.FirstName = FirstName;
            user.LastName = LastName;
            if (pswHash != null && pswHash.Any())
                user.PasswordHash = pswHash;
            if (pswSalt != null && pswSalt.Any())
                user.PasswordSalt = pswSalt;
        }
    }

    public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            this.RuleFor(x => x.FirstName)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(2)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(50)
                    .WithMessage(ErrorMessage.Validation.MaxLength());

            this.RuleFor(x => x.LastName)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(2)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(50)
                    .WithMessage(ErrorMessage.Validation.MaxLength());

            this.RuleFor(x => x.NewPassword)
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MinimumLength(5)
                    .WithMessage(ErrorMessage.Validation.MinLength())
                .MaximumLength(100)
                    .WithMessage(ErrorMessage.Validation.MaxLength())
                .NotEqual(x => x.OldPassword)
                    .WithMessage(ErrorMessage.Validation.NewPasswordCannotBeSame)
                .When(x => !string.IsNullOrEmpty(x.NewPassword));

            this.RuleFor(x => x.RepeatPassword)
                .NotEmpty()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MaximumLength(100)
                    .WithMessage(ErrorMessage.Validation.MaxLength())
                .Equal(x => x.NewPassword)
                    .WithMessage(ErrorMessage.Validation.PasswordsNotMatches)
                .When(x => !string.IsNullOrEmpty(x.RepeatPassword));
        }
    }
}