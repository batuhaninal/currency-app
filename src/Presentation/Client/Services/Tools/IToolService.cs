using Client.Models.Categories;
using Client.Models.Commons;
using Client.Models.Currencies;

namespace Client.Services.Tools
{
    public interface IToolService
    {
        Task<BaseResult<List<CategoryToolResponse>>> CategoryTools(ToolRequestParameter parameter, CancellationToken cancellationToken = default);
        Task<BaseResult<List<CurrencyToolResponse>>> CurrencyTools(ToolRequestParameter parameter, CancellationToken cancellationToken = default);
    }
}