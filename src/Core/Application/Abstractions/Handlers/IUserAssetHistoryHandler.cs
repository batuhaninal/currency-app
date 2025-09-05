using Application.CQRS.Commands.UserAssetHistories.SaveUserAssetHistory;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.UserAssetHistories.ItemList;
using Application.CQRS.Queries.UserAssetHistories.List;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IUserAssetHistoryHandler
    {
        Task<IResult> List(UserAssetHistoryListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> ItemList(UserAssetItemHistoryListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> Save(SaveUserAssetHistoryCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
    }
}