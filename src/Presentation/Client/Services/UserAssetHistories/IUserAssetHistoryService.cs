using Client.Models.Commons;
using Client.Models.UserAssetHistories;
using Client.Models.UserAssetHistories.RequestParameters;
using Client.Services.Commons;

namespace Client.Services.UserAssetHistories
{
    public interface IUserAssetHistoryService
    {
        Task<BaseResult<PaginationResult<UserAssetHistoryItemResponse>>> ListAsync(UserAssetHistoryRequestParameter parameter, CancellationToken cancellationToken = default);
        Task<BaseResult<List<UserAssetItemHistoryItemResponse>>> ItemListAsync(int userAssetHistoryId, CancellationToken cancellationToken = default);
    }

    public sealed class UserAssetHistoryService : BaseService, IUserAssetHistoryService
    {
        private readonly HttpClient _httpClient;

        public UserAssetHistoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<List<UserAssetItemHistoryItemResponse>>> ItemListAsync(int userAssetHistoryId, CancellationToken cancellationToken = default)
        {
            var result = await this.GetAsync<List<UserAssetItemHistoryItemResponse>>(_httpClient, $"item/{userAssetHistoryId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<PaginationResult<UserAssetHistoryItemResponse>>> ListAsync(UserAssetHistoryRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            var result = await this.GetAsync<PaginationResult<UserAssetHistoryItemResponse>>(_httpClient, "", parameter.ToQueryString(), cancellationToken);
            return result;
        }
    }
}