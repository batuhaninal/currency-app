using Application.Abstractions.Commons.Results;
using Application.Abstractions.Handlers;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.UserAssetHistories.ItemList;
using Application.CQRS.Queries.UserAssetHistories.List;

namespace API.Handlers.v1
{
    public sealed class UserAssetHistoryHandler : IUserAssetHistoryHandler
    {
        public async Task<IResult> ItemList(UserAssetItemHistoryListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<UserAssetItemHistoryListQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> List(UserAssetHistoryListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<UserAssetHistoryListQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }
    }
}