using Application.CQRS.Commands.Users.Login;
using Application.CQRS.Commands.Users.Register;
using Application.CQRS.Commons.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Abstractions.Handlers
{
    public interface IAuthHandler
    {
        Task<IResult> Register(RegisterCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> Login(LoginCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
    }
}