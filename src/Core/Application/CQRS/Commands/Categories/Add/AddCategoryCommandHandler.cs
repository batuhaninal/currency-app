using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Domain;

namespace Application.CQRS.Commands.Categories.Add
{
    public sealed class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(AddCategoryCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CategoryRule.CheckTitleValidAsync(command.Title, cancellationToken);

            if (!result.Success)
                return result;

            Category category = await _unitOfWork.CategoryWriteRepository.CreateAsync(command.ToDomain(), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(201, true, new CategoryItemDto(category.Id, category.Title, category.IsActive));

        }
    }
}