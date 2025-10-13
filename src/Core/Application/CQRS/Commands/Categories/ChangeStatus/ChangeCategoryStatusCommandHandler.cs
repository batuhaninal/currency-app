using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Categories.ChangeStatus
{
    public sealed class ChangeCategoryStatusCommandHandler : ICommandHandler<ChangeCategoryStatusCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public ChangeCategoryStatusCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(ChangeCategoryStatusCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CategoryRule.CheckExistAsync(command.CategoryId, cancellationToken);

            if (!result.Success)
                return result;

            await _unitOfWork
                .CategoryWriteRepository
                .Table
                .Where(x=> x.Id == command.CategoryId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(k => k.IsActive, v => !v.IsActive),
                    cancellationToken
                );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CategoryPrefix);

            return new ResultDto(203, true);
        }
    }
}