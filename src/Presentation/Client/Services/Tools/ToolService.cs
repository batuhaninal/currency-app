using Client.Models.Categories;
using Client.Models.Commons;
using Client.Models.Currencies;
using Client.Services.Commons;

namespace Client.Services.Tools
{
    public sealed class ToolService : BaseService, IToolService
    {
        private readonly HttpClient _httpClient;

        public ToolService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<List<CategoryToolResponse>>> CategoryTools(ToolRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<List<CategoryToolResponse>> result = await this.GetAsync<List<CategoryToolResponse>>(_httpClient, "category-list", parameter.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<List<CurrencyToolResponse>>> CurrencyTools(ToolRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<List<CurrencyToolResponse>> result = await this.GetAsync<List<CurrencyToolResponse>>(_httpClient, "currency-list", parameter.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<List<CurrencyToolResponse>>> CurrencyToolWithoutFavs(bool isBroadcast = false, CancellationToken cancellationToken = default)
        {
            List<KeyValuePair<string, object>> parameter = new();
            parameter.Add(new KeyValuePair<string, object>("isBroadcast", isBroadcast));
            BaseResult<List<CurrencyToolResponse>> result = await this.GetAsync<List<CurrencyToolResponse>>(_httpClient, "currency-list-without-favs", parameter.ToArray(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<List<CurrencyTagToolResponse>>> CurrencyTagListAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            List<KeyValuePair<string, object>> parameter = new();
            parameter.Add(new KeyValuePair<string, object>("currencyId", currencyId));
            var result = await this.GetAsync<List<CurrencyTagToolResponse>>(_httpClient, "currency-tag-tool", parameter.ToArray(), cancellationToken);
            return result;
        }
    }
}