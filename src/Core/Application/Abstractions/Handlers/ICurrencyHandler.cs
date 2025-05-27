using Application.CQRS.Commands.Currencies.Add;
using Application.CQRS.Commands.Currencies.ChangeStatus;
using Application.CQRS.Commands.Currencies.Update;
using Application.CQRS.Commands.Currencies.UpdateValue;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Currencies.List;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface ICurrencyHandler
    {
        Task<IResult> Add(AddCurrencyCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> Update(UpdateCurrencyCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> ChangeStatus(ChangeCurrencyStatusCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> UpdateValue(UpdateCurrencyValueCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> List(CurrencyListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken); 
    }
}