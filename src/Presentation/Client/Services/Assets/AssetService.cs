using Client.Models.Assets;
using Client.Models.Assets.RequestParameters;
using Client.Models.Commons;
using Client.Services.Commons;

namespace Client.Services.Assets
{
    public sealed class AssetService : BaseService, IAssetService
    {
        private readonly HttpClient _httpClient;

        public AssetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<NoContent>> AddAsync(AssetAddInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PostAsync<AssetAddInput, NoContent>(_httpClient, "", input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> DeleteAsync(int assetId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await base.DeleteAsync<NoContent>(_httpClient, $"{assetId}", cancellationToken);
            return result;
        }

        public async Task<BaseResult<AssetForUpdateResponse>> GetForUpdateAsync(int assetId, CancellationToken cancellationToken = default)
        {
            BaseResult<AssetForUpdateResponse> result = await this.GetAsync<AssetForUpdateResponse>(_httpClient, $"for-update/{assetId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdateAsync(int assetId, AssetUpdateInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PutAsync<AssetUpdateInput, NoContent>(_httpClient, $"{assetId}", input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<UserAssetInfoResponse>> UserAssetInfoAsync(int assetId, CancellationToken cancellationToken = default)
        {
            BaseResult<UserAssetInfoResponse> result = await this.GetAsync<UserAssetInfoResponse>(_httpClient, $"users-asset-info/{assetId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<PaginationResult<UserAssetItemResponse>>> UserAssetsAsync(AssetRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<PaginationResult<UserAssetItemResponse>> result = await this.GetAsync<PaginationResult<UserAssetItemResponse>>(_httpClient, "", parameter.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<List<UserAssetSummaryResponse>>> UserAssetSummaryAsync(CancellationToken cancellationToken = default)
        {
            BaseResult<List<UserAssetSummaryResponse>> result = await this.GetAsync<List<UserAssetSummaryResponse>>(_httpClient, "summary", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<List<UserAssetWithGroupResponse>>> UserAssetsWithGroupAsync(CancellationToken cancellationToken = default)
        {
            BaseResult<List<UserAssetWithGroupResponse>> result = await this.GetAsync<List<UserAssetWithGroupResponse>>(_httpClient, "user-asset-group", null, cancellationToken);
            return result;
        }
    }
}