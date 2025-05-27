using Client.Models.Categories;
using Client.Models.Categories.RequestParameters;
using Client.Models.Commons;
using Client.Services.Commons;

namespace Client.Services.Categories
{
    public sealed class CategoryService : BaseService, ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<NoContent>> AddAsync(CategoryInput categoryInput, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PostAsync<CategoryInput, NoContent>(_httpClient, "", categoryInput, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> ChangeStatusAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PatchAsync<NoContent, NoContent>(_httpClient, $"status/{categoryId}", new(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> DeleteAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.DeleteAsync<NoContent>(_httpClient, $"{categoryId}", cancellationToken);
            return result;
        }

        public async Task<BaseResult<CategoryInfoResponse>> InfoAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            BaseResult<CategoryInfoResponse> result = await this.GetAsync<CategoryInfoResponse>(_httpClient, $"{categoryId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<PaginationResult<CategoryItemResponse>>> ListAsync(CategoryBaseRequestParameter parameters, CancellationToken cancellationToken = default)
        {
            BaseResult<PaginationResult<CategoryItemResponse>> result = await this.GetAsync<PaginationResult<CategoryItemResponse>>(_httpClient, "", parameters.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdateAsync(int categoryId, CategoryInput categoryInput, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PutAsync<CategoryInput, NoContent>(_httpClient, $"{categoryId}", categoryInput, cancellationToken);
            return result;
        }
    }
}