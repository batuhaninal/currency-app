using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.Abstractions.Repositories.UserAssetHistories.Extensions;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using Application.Models.DTOs.UserAssetHistories;
using Application.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.UserAssetHistories.List
{
    public sealed class UserAssetHistoryListQueryHandler : IQueryHandler<UserAssetHistoryListQuery, IBaseResult>
    {
        private readonly IUserTokenService _userTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public UserAssetHistoryListQueryHandler(IUserTokenService userTokenService, IUnitOfWork unitOfWork)
        {
            _userTokenService = userTokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResult> Handle(UserAssetHistoryListQuery query, CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork
                .UserAssetHistoryReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.UserId == _userTokenService.UserId)
                .FilterAllConditions(query)
                .Select(x=> new UserAssetHistoryItemDto(
                    x.Id,
                    x.Date,
                    x.TotalPurchasePrice,
                    x.TotalSalePrice,
                    x.TotalCurrentPurchasePrice,
                    x.TotalCurrentSalePrice,
                    x.TotalCostPurchasePrice,
                    x.TotalCostSalePrice,
                    x.CreatedDate
                ))
                .ToPaginatedListDtoAsync(query, cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}