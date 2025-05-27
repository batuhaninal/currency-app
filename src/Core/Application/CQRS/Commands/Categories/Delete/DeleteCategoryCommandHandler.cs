using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;

namespace Application.CQRS.Commands.Categories.Delete
{
    public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CategoryRule.CheckExistAsync(command.CategoryId, cancellationToken);

            if (!result.Success)
                return result;

            await _unitOfWork
                .CategoryWriteRepository
                .RemoveAsync(command.CategoryId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto(200, true);
        }
    }
}