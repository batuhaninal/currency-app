using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Categories;
using Application.Models.DTOs.Commons.Results;
using Domain.Entities;

namespace Application.CQRS.Commands.Categories.Add
{
    public sealed class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(AddCategoryCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CategoryRule.CheckTitleValidAsync(command.Title, cancellationToken);

            if (!result.Success)
                return result;

            Category category = await _unitOfWork.CategoryWriteRepository.CreateAsync(command.ToDomain(), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CategoryPrefix);

            return new ResultDto(201, true, new CategoryItemDto(category.Id, category.Title, category.IsActive));

        }
    }
}