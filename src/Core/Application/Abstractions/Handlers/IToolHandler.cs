using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Tools;
using Application.CQRS.Queries.Tools.GetCurrencyToolList;
using Application.CQRS.Queries.Tools.ToolWithoutFavs;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface IToolHandler
    {
        Task<IResult> CategoryToolList(GetCategoryToolListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> CurrencyToolList(GetCurrencyToolListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> CurrencyToolWithoutFavs(CurrencyToolWithoutFavsQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
    }
}