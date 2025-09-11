using Application.CQRS.Commands.UserCurrencyFollows.Add;
using Application.CQRS.Commands.UserCurrencyFollows.Delete;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.UserCurrencyFollows.UserCurrencyFavList;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IUserCurrencyFollowHandler
    {
        Task<IResult> AddAsync(AddUserCurrencyFollowCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> DeleteAsync(DeleteUserCurrencyFollowCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> FavListAsync(UserCurrencyFavListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
    }
}