using Application.CQRS.Commands.Assets.AddAsset;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Assets.GetUserAssetHistory;
using Application.CQRS.Queries.Assets.GetUserAssetInfo;
using Application.CQRS.Queries.Assets.GetUsersAssets;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IAssetHandler
    {
        Task<IResult> Add(AddAssetCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> GetUsersAssets(GetUsersAssetsQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> GetUsersAssetInfo(GetUsersAssetInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> GetUsersAssetHistory(GetUserAssetHistoryQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
    }
}