using Application.CQRS.Commons.Interfaces;
using Domain.Entities;

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
}