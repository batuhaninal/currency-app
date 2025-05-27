using Application.Abstractions.Commons.Results;
using Application.Abstractions.Repositories.Assets;
using Application.Abstractions.Rules;
using Application.Models.Constants.Messages;
using Application.Models.DTOs.Commons.Results;

namespace Persistence.Services.Rules
{
    public sealed class AssetRule : IAssetRule
    {
        private readonly IAssetReadRepository _assetReadRepository;

        public AssetRule(IAssetReadRepository assetReadRepository)
        {
            _assetReadRepository = assetReadRepository;
        }

        public async Task<IBaseResult> CheckExistAsync(int id, CancellationToken cancellationToken = default)
        {
            bool result = await _assetReadRepository.AnyAsync(x=> x.Id == id, cancellationToken);

            return new ResultDto(400, result, null, ErrorMessage.ASSETEXIST);
        }

        public async Task<IBaseResult> CheckExistAsync(int id, int userId, CancellationToken cancellationToken = default)
        {
            bool result = await _assetReadRepository.AnyAsync(x=> x.Id == id && x.UserId == userId, cancellationToken);

            return new ResultDto(400, result, null, ErrorMessage.ASSETEXIST);
        }

        public async Task<IBaseResult> CheckUserHasAnyCurrencyAsset(int currencyId, int userId, CancellationToken cancellationToken = default)
        {
            bool result = await _assetReadRepository.AnyAsync(x=> x.CurrencyId == currencyId && x.UserId == userId, cancellationToken);

            return new ResultDto(400, result, null, ErrorMessage.CURRENCYEXIST);
        }
    }
}