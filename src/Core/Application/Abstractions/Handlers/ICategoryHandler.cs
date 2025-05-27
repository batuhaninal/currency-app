using Application.CQRS.Commands.Categories.Add;
using Application.CQRS.Commands.Categories.ChangeStatus;
using Application.CQRS.Commands.Categories.Delete;
using Application.CQRS.Commands.Categories.Update;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Categories.Info;
using Application.CQRS.Queries.Categories.List;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Handlers
{
    public interface ICategoryHandler
    {
        Task<IResult> AddAsync(AddCategoryCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> UpdateAsync(UpdateCategoryCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> ChangeStatusAsync(ChangeCategoryStatusCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> DeleteAsync(DeleteCategoryCommand command, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> ListAsync(CategoryListQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
        Task<IResult> Get(CategoryInfoQuery query, Dispatcher dispatcher, CancellationToken cancellationToken);
    }
}