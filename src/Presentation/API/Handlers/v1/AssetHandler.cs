using Application.Abstractions.Commons.Results;
using Application.Abstractions.Handlers;
using Application.CQRS.Commands.Assets.AddAsset;
using Application.CQRS.Commands.Assets.Delete;
using Application.CQRS.Commands.Assets.Update;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Assets.GetForUpdate;
using Application.CQRS.Queries.Assets.GetUserAssetHistory;
using Application.CQRS.Queries.Assets.GetUserAssetInfo;
using Application.CQRS.Queries.Assets.GetUsersAssets;
using Application.CQRS.Queries.Assets.UserAssetItems;
using Application.CQRS.Queries.Assets.UserSummary;

namespace API.Handlers.v1
{
    public sealed class AssetHandler : IAssetHandler
    {
        public async Task<IResult> Add(AddAssetCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<AddAssetCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> GetUserAssetWithGroup(GetUsersAssetWithGroupQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<GetUsersAssetWithGroupQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> GetUsersAssetInfo(GetUsersAssetInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<GetUsersAssetInfoQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> GetUsersAssetHistory(GetUserAssetHistoryQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<GetUserAssetHistoryQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> UserAssetItems(UserAssetItemsQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<UserAssetItemsQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> Update(UpdateAssetCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<UpdateAssetCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> Delete(DeleteAssetCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<DeleteAssetCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> GetForUpdate(GetForUpdateAssetQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<GetForUpdateAssetQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> UserSummary(UserSummaryAssetQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<UserSummaryAssetQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }
    }
}