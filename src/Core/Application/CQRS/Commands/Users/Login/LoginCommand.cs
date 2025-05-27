using Application.CQRS.Commons.Interfaces;

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
}