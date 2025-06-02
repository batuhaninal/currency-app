using Client.Models.Assets;
using Client.Models.Assets.RequestParameters;
using Client.Models.Commons;

namespace Client.Services.Assets
{
    public interface IAssetService
    {
        Task<BaseResult<List<UserAssetWithGroupResponse>>> UserAssetsWithGroupAsync(CancellationToken cancellationToken = default);
        Task<BaseResult<UserAssetInfoResponse>> UserAssetInfoAsync(int assetId, CancellationToken cancellationToken = default);
        Task<BaseResult<PaginationResult<UserAssetItemResponse>>> UserAssetsAsync(AssetRequestParameter parameter, CancellationToken cancellationToken = default);
        Task<BaseResult<List<UserAssetSummaryResponse>>> UserAssetSummaryAsync(CancellationToken cancellationToken = default);
        Task<BaseResult<AssetForUpdateResponse>> GetForUpdateAsync(int assetId, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> AddAsync(AssetAddInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> UpdateAsync(int assetId, AssetUpdateInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> DeleteAsync(int assetId, CancellationToken cancellationToken = default);
    }
}