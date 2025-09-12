using Client.Models.Commons;
using Client.Models.UserCurrencyFollows;
using Client.Models.UserCurrencyFollows.RequestParameters;
using Client.Services.Commons;

namespace Client.Services.UserCurrencyFollows
{
    public interface IUserCurrencyFollowService
    {
        Task<BaseResult<NoContent>> AddAsync(CurrencyFollowInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> ChangeStatusAsync(int userCurrencyFollowId, ChangeCurrencyFollowInput input, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> DeleteAsync(int currencyId, CancellationToken cancellationToken = default);
        Task<BaseResult<int[]>> TopFollowList(BroadcastParameters parameters, CancellationToken cancellationToken = default);
        Task<BaseResult<PaginationResult<UserCurrencyFollowItemResponse>>> ListAsync(UserCurrencyFollowRequestParameter parameters, CancellationToken cancellationToken = default);
        Task<BaseResult<UserCurrencyFollowInfoResponse>> InfoAsync(int userCurrencyFollowId, CancellationToken cancellationToken = default);
    }

    public sealed class UserCurrencyFollowService : BaseService, IUserCurrencyFollowService
    {
        private const string panel = "panel";
        private readonly HttpClient _httpClient;

        public UserCurrencyFollowService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<NoContent>> AddAsync(CurrencyFollowInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PostAsync<CurrencyFollowInput, NoContent>(_httpClient, "", input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> ChangeStatusAsync(int userCurrencyFollowId, ChangeCurrencyFollowInput input, CancellationToken cancellationToken = default)
        {
            input.UserCurrencyFollowId = userCurrencyFollowId;
            BaseResult<NoContent> result = await this.PutAsync<ChangeCurrencyFollowInput, NoContent>(_httpClient, $"{input.UserCurrencyFollowId}", input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> DeleteAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.DeleteAsync<NoContent>(_httpClient, $"{currencyId}", cancellationToken);
            return result;
        }

        public async Task<BaseResult<UserCurrencyFollowInfoResponse>> InfoAsync(int userCurrencyFollowId, CancellationToken cancellationToken = default)
        {
            BaseResult<UserCurrencyFollowInfoResponse> result = await this.GetAsync<UserCurrencyFollowInfoResponse>(_httpClient, $"{userCurrencyFollowId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<PaginationResult<UserCurrencyFollowItemResponse>>> ListAsync(UserCurrencyFollowRequestParameter parameters, CancellationToken cancellationToken = default)
        {
            BaseResult<PaginationResult<UserCurrencyFollowItemResponse>> result = await this.GetAsync<PaginationResult<UserCurrencyFollowItemResponse>>(_httpClient, "", parameters.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<int[]>> TopFollowList(BroadcastParameters parameters, CancellationToken cancellationToken = default)
        {
            BaseResult<int[]> result = await this.GetAsync<int[]>(_httpClient, $"fav-list", parameters.ToQueryString(), cancellationToken);
            return result;
        }
    }
}