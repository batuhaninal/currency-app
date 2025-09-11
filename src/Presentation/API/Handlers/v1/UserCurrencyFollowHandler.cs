using Application.Abstractions.Commons.Results;
using Application.Abstractions.Handlers;
using Application.CQRS.Commands.UserCurrencyFollows.Add;
using Application.CQRS.Commands.UserCurrencyFollows.Delete;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.UserCurrencyFollows.UserCurrencyFavList;

namespace API.Handlers.v1
{
    public sealed class UserCurrencyFollowHandler : IUserCurrencyFollowHandler
    {
        public async Task<IResult> AddAsync(AddUserCurrencyFollowCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<AddUserCurrencyFollowCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> FavListAsync(UserCurrencyFavListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<UserCurrencyFavListQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> DeleteAsync(DeleteUserCurrencyFollowCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<DeleteUserCurrencyFollowCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }
    }
}