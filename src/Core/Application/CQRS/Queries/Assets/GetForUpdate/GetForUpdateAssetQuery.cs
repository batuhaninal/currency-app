using Application.Abstractions.Commons.Results;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Repositories.Commons;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Assets;
using Application.Models.DTOs.Commons.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries.Assets.GetForUpdate
{
    public sealed record GetForUpdateAssetQuery : IQuery
    {
        public GetForUpdateAssetQuery()
        {

        }

        public GetForUpdateAssetQuery(int assetId)
        {
            AssetId = assetId;
        }

        public int AssetId { get; init; }
    }

    public sealed class GetForUpdateAssetQueryHandler : IQueryHandler<GetForUpdateAssetQuery, IBaseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokenService _userTokenService;

        public GetForUpdateAssetQueryHandler(IUnitOfWork unitOfWork, IUserTokenService userTokenService)
        {
            _unitOfWork = unitOfWork;
            _userTokenService = userTokenService;
        }

        public async Task<IBaseResult> Handle(GetForUpdateAssetQuery query, CancellationToken cancellationToken = default)
        {
            IBaseResult result = await _unitOfWork.AssetRule.CheckExistAsync(query.AssetId, _userTokenService.UserId, cancellationToken);

            if (!result.Success)
                return result;

            var data = await _unitOfWork
                .AssetReadRepository
                .Table
                .AsNoTracking()
                .Where(x => x.Id == query.AssetId && x.UserId == _userTokenService.UserId)
                .Select(x => new AssetForUpdateDto(x.Id, x.Count, x.PurchasePrice, x.SalePrice, x.PurchaseDate))
                .FirstOrDefaultAsync(cancellationToken);

            return new ResultDto(200, true, data);
        }
    }
}