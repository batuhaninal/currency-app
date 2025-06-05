using Client.Models.Commons;
using Client.Models.Users;
using Client.Services.Commons;

namespace Client.Services.Users
{
    public interface IUserService
    {
        Task<BaseResult<UserProfileResponse>> ProfileAsync(CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> UpdateProfileAsync(UserProfileInput input, CancellationToken cancellationToken = default);
    }

    public sealed class UserService : BaseService, IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<UserProfileResponse>> ProfileAsync(CancellationToken cancellationToken = default)
        {
            BaseResult<UserProfileResponse> result = await this.GetAsync<UserProfileResponse>(_httpClient, "profile", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdateProfileAsync(UserProfileInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PutAsync<UserProfileInput,NoContent>(_httpClient, "update-profile", input, cancellationToken);
            return result;
        }
    }
}