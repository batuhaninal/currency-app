using Application.CQRS.Commands.UpdateProfile;
using Application.CQRS.Commons.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IUserHandler
    {
        Task<IResult> GetProfile(Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> UpdateProfile(UpdateProfileCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
    }   
}