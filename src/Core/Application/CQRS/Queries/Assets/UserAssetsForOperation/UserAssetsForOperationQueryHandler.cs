using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Assets.UserAssetsForOperationQuery
{
    public sealed class UserAssetsForOperationQueryHandler : IQueryHandler<UserAssetsForOperationQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public UserAssetsForOperationQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UserAssetsForOperationQuery query, CancellationToken cancellationToken = default)
        {
            var currencies = await _unitOfWork
                .AssetReadRepository
                .Table
                .AsNoTracking()
                .Include(x => x.Currency)
                .Where(x => x.UserId == _userTokenService.UserId)
                .GroupBy(x => x.CurrencyId)
                .Select(x => new UserAssetForOperationDto(
                    x.Key,
                    x.First().Currency.Title,
                    x.First().Currency.SubTitle,
                    x.Sum(a => a.Count),
                    x.First().Currency.PurchasePrice,
                    x.First().Currency.SalePrice
                ))
                .ToListAsync(cancellationToken);

            return new ResultDto(200, true, currencies);                
        }
    }
}