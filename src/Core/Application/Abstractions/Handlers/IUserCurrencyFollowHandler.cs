using Application.CQRS.Commands.UserCurrencyFollows.Add;
using Application.CQRS.Commands.UserCurrencyFollows.ChangeStatus;
using Application.CQRS.Commands.UserCurrencyFollows.Delete;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.UserCurrencyFollows.Info;
using Application.CQRS.Queries.UserCurrencyFollows.List;
using Application.CQRS.Queries.UserCurrencyFollows.UserCurrencyFavList;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IUserCurrencyFollowHandler
    {
        Task<IResult> AddAsync(AddUserCurrencyFollowCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> DeleteAsync(DeleteUserCurrencyFollowCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> ChangeStatusAsync(ChangeUserCurrencyFollowStatusCommand command,Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> FavListAsync(UserCurrencyFavListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> ListAsync(UserCurrencyListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> InfoAsync(UserCurrencyFollowInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);    
    }
}