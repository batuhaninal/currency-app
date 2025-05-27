using Client.Models.Categories;
using Client.Models.Commons;

namespace Client.Services.Tools
{
    public interface IToolService
    {
        Task<BaseResult<List<CategoryToolResponse>>> CategoryTools(ToolRequestParameter parameter, CancellationToken cancellationToken = default);
    }
}