using Application.Abstractions.Commons.Results;
using Application.Abstractions.Handlers;
using Application.CQRS.Commands.UserCurrencyFollows.Add;
using Application.CQRS.Commands.UserCurrencyFollows.AddRange;
using Application.CQRS.Commands.UserCurrencyFollows.ChangeStatus;
using Application.CQRS.Commands.UserCurrencyFollows.Delete;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.UserCurrencyFollows.Info;
using Application.CQRS.Queries.UserCurrencyFollows.List;
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

        public async Task<IResult> ChangeStatusAsync(ChangeUserCurrencyFollowStatusCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<ChangeUserCurrencyFollowStatusCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> ListAsync(UserCurrencyListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<UserCurrencyListQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> InfoAsync(UserCurrencyFollowInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<UserCurrencyFollowInfoQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> AddRangeAsync(AddRangeUserCurrencyFollowCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<AddRangeUserCurrencyFollowCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }
    }
}