using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Domain;

namespace Application.CQRS.Commands.Categories.Update
{
    public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CategoryRule.CheckExistAsync(command.CategoryId, cancellationToken);
            if (!result.Success)
                return result;

            result = await _unitOfWork.CategoryRule.CheckTitleValidAsync(command.CategoryId, command.Title, cancellationToken);

            if (!result.Success)
                return result;

            Category category = (await _unitOfWork
                .CategoryReadRepository
                .GetByIdAsync(command.CategoryId, true, cancellationToken))!;

            command.ToUpdate(ref category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(200, true, new CategoryItemDto(category.Id, category.Title, category.IsActive));
        }
    }
}