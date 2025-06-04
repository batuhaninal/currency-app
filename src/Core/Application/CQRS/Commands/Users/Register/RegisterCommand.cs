using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.Messages;
using Domain.Entities;
using FluentValidation;

namespace Application.CQRS.Commands.Users.Register
{
    public sealed class RegisterCommand : ICommand
    {
        // public Guid Id => Guid.NewGuid();
        public RegisterCommand()
        {

        }

        public RegisterCommand(string username, string password, string firstName, string lastName)
        {
            Username = username.TrimStart().TrimEnd();
            Password = password;
            FirstName = firstName.TrimStart().TrimEnd();
            LastName = lastName.TrimStart().TrimEnd();
        }

        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string RepeatPassword { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;

        internal User ToUser(ref byte[] pswHash, ref byte[] pswSalt)
        {
            DateTime now = DateTime.UtcNow;
            return new User
            {
                FirstName = this.FirstName.TrimStart().TrimEnd(),
                LastName = this.LastName.TrimStart().TrimEnd(),
                Email = this.Username.TrimStart().TrimEnd(),
                PasswordHash = pswHash,
                PasswordSalt = pswSalt,
                CreatedDate = now,
                UpdatedDate = now,
                IsActive = true,
            };
        }
    }

    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
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

            this.RuleFor(x => x.RepeatPassword)
                .NotEmpty()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .NotNull()
                    .WithMessage(ErrorMessage.Validation.NotNull())
                .MaximumLength(100)
                    .WithMessage(ErrorMessage.Validation.MaxLength())
                .Equal(x => x.Password)
                    .WithMessage(ErrorMessage.Validation.PasswordsNotMatches);
        }
    }
}