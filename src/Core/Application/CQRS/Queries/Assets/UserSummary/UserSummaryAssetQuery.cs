using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Assets.UserSummary
{
    public sealed record UserSummaryAssetQuery : IQuery
    {
        public UserSummaryAssetQuery()
        {

        }
    }

    public sealed class UserSummaryAssetQueryHandler : IQueryHandler<UserSummaryAssetQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public UserSummaryAssetQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(UserSummaryAssetQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .AssetReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.UserId == _userTokenService.UserId)
                .Include(x => x.Currency)
                .OrderBy(x => x.CurrencyId)
                .GroupBy(key => key.CurrencyId)
                .Select(x => new UserAssetSummaryDto(
                    x.Key,
                    x.First().Currency.Title,
                    x.Sum(v=> v.Count),
                    x.Sum(v => v.Currency.PurchasePrice * v.Count),
                    x.Sum(v => v.Currency.SalePrice * v.Count),
                    x.Sum(v => v.CurrentPurchasePrice * v.Count),
                    x.Sum(v => v.CurrentSalePrice * v.Count),
                    x.Sum(v => v.PurchasePrice * v.Count),
                    x.Sum(v => v.SalePrice * v.Count)
                ))
                .ToListAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}