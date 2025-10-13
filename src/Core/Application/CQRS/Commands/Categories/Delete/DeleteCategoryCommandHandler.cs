using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Commons.Results;

namespace Application.CQRS.Commands.Categories.Delete
{
    public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
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

            await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CategoryPrefix);

            return new ResultDto(200, true);
        }
    }
}