using Application.Abstractions.Commons.Results;
using Application.Abstractions.Handlers;
using Application.CQRS.Commands.Currencies.Add;
using Application.CQRS.Commands.Currencies.ChangeStatus;
using Application.CQRS.Commands.Currencies.Delete;
using Application.CQRS.Commands.Currencies.Update;
using Application.CQRS.Commands.Currencies.UpdateValue;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Currencies.Calculator;
using Application.CQRS.Queries.Currencies.Info;
using Application.CQRS.Queries.Currencies.List;
using Application.CQRS.Queries.Currencies.WithHistoryInfo;
using Application.CQRS.Queries.PriceInfo;

namespace API.Handlers.v1
{
    public sealed class CurrencyHandler : ICurrencyHandler
    {
        public async Task<IResult> Add(AddCurrencyCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<AddCurrencyCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> Calculator(CalculatorQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<CalculatorQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> ChangeStatus(ChangeCurrencyStatusCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<ChangeCurrencyStatusCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> Delete(DeleteCurrencyCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<DeleteCurrencyCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> HistoryInfo(CurrencyWithHistoryInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<CurrencyWithHistoryInfoQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> Info(CurrencyInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<CurrencyInfoQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> List(CurrencyListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<CurrencyListQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> PriceInfo(CurrencyPriceInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<CurrencyPriceInfoQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> Update(UpdateCurrencyCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<UpdateCurrencyCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> UpdateValue(UpdateCurrencyValueCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<UpdateCurrencyValueCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }
    }
}