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
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IAssetHandler
    {
        Task<IResult> Add(AddAssetCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> Update(UpdateAssetCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> Delete(DeleteAssetCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> GetUserAssetWithGroup(GetUsersAssetWithGroupQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> GetUsersAssetInfo(GetUsersAssetInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> GetForUpdate(GetForUpdateAssetQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> GetUsersAssetHistory(GetUserAssetHistoryQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> UserAssetItems(UserAssetItemsQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> UserSummary(UserSummaryAssetQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
    }
}