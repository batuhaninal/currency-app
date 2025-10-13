using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Commons.Results;

namespace Application.CQRS.Commands.Currencies.Delete
{
    public sealed class DeleteCurrencyCommandHandler : ICommandHandler<DeleteCurrencyCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public DeleteCurrencyCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(DeleteCurrencyCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(command.CurrencyId, cancellationToken);
            if (!result.Success)
                return result;

            await _unitOfWork
                .CurrencyWriteRepository
                .RemoveAsync(command.CurrencyId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CurrencyPrefix);

            return new ResultDto(200, true);
        }
    }
}