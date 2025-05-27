using Client.Models.Categories;
using Client.Models.Categories.RequestParameters;
using Client.Models.Commons;

namespace Client.Services.Categories
{
    public interface ICategoryService
    {
        Task<BaseResult<PaginationResult<CategoryItemResponse>>> ListAsync(CategoryBaseRequestParameter parameters, CancellationToken cancellationToken = default);
        Task<BaseResult<CategoryInfoResponse>> InfoAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> UpdateAsync(int categoryId, CategoryInput categoryInput, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> AddAsync(CategoryInput categoryInput, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> ChangeStatusAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<BaseResult<NoContent>> DeleteAsync(int categoryId, CancellationToken cancellationToken = default);
    }
}