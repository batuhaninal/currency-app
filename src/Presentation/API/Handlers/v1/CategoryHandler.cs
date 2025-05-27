using Application.Abstractions.Commons.Results;
using Application.Abstractions.Handlers;
using Application.CQRS.Commands.Categories.Add;
using Application.CQRS.Commands.Categories.ChangeStatus;
using Application.CQRS.Commands.Categories.Delete;
using Application.CQRS.Commands.Categories.Update;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Categories.Info;
using Application.CQRS.Queries.Categories.List;

namespace API.Handlers.v1
{
    public sealed class CategoryHandler : ICategoryHandler
    {
        public async Task<IResult> AddAsync(AddCategoryCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<AddCategoryCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> ChangeStatusAsync(ChangeCategoryStatusCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<ChangeCategoryStatusCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> DeleteAsync(DeleteCategoryCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<DeleteCategoryCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> Get(CategoryInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<CategoryInfoQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> ListAsync(CategoryListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<CategoryListQuery, IBaseResult>(query, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }

        public async Task<IResult> UpdateAsync(UpdateCategoryCommand command, Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendCommandAsync<UpdateCategoryCommand, IBaseResult>(command, cancellationToken);
            return Results.Json(result, statusCode: result.StatusCode);
        }
    }
}