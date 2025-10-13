using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.Constants.CachePrefixes;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Commands.Currencies.ChangeStatus
{
    public sealed class ChangeCurrencyStatusCommandHandler : ICommandHandler<ChangeCurrencyStatusCommand, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public ChangeCurrencyStatusCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<IBaseResult> Handle(ChangeCurrencyStatusCommand command, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.CurrencyRule.CheckExistAsync(command.CurrencyId, cancellationToken);

            if (result.Success)
            {
                await _unitOfWork
                    .CurrencyWriteRepository
                    .Table
                    .Where(x => x.Id == command.CurrencyId)
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(p => p.IsActive, v => !v.IsActive),
                        cancellationToken
                    );

                await _cacheService.DeleteAllWithPrefixAsync(CachePrefix.CurrencyPrefix);

                result = new ResultDto(203, true);
            }

            return result;
        }
    }
}