using Client.Models.Commons;
using Client.Models.Currencies;
using Client.Models.Currencies.RequestParameters;
using Client.Services.Commons;

namespace Client.Services.Currencies
{
    public sealed class CurrencyService : BaseService, ICurrencyService
    {
        private const string panel = "panel";
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<NoContent>> AddAsync(CurrencyInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PostAsync<CurrencyInput, NoContent>(_httpClient, panel, input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<List<CalculatorItemResponse>>> CalculatorAsync(CancellationToken cancellationToken = default)
        {
            BaseResult<List<CalculatorItemResponse>> result = await this.GetAsync<List<CalculatorItemResponse>>(_httpClient, "calculator", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> ChangeStatusAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PatchAsync<NoContent, NoContent>(_httpClient, $"{panel}/status/{currencyId}", new(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> DeleteAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.DeleteAsync<NoContent>(_httpClient, $"{panel}/{currencyId}", cancellationToken);
            return result;
        }

        public async Task<BaseResult<EUCurrencyInfoResponse>> EUInfoAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<EUCurrencyInfoResponse> result = await this.GetAsync<EUCurrencyInfoResponse>(_httpClient, $"{currencyId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<PaginationResult<EUCurrencyItemResponse>>> EUListAsync(CurrencyRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<PaginationResult<EUCurrencyItemResponse>> result = await this.GetAsync<PaginationResult<EUCurrencyItemResponse>>(_httpClient, "", parameter.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<CurrencyWithHistoryResponse>> HistoryInfoAsync(int currencyId, CurrencyHistoryRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<CurrencyWithHistoryResponse> result = await this.GetAsync<CurrencyWithHistoryResponse>(_httpClient, $"{panel}/history-info/{currencyId}", parameter.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<CurrencyInfoResponse>> InfoAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<CurrencyInfoResponse> result = await this.GetAsync<CurrencyInfoResponse>(_httpClient, $"{panel}/{currencyId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<PaginationResult<CurrencyItemResponse>>> ListAsync(CurrencyRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<PaginationResult<CurrencyItemResponse>> result = await this.GetAsync<PaginationResult<CurrencyItemResponse>>(_httpClient, panel, parameter.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<CurrencyPriceInfoResponse>> PriceInfoAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<CurrencyPriceInfoResponse> result = await this.GetAsync<CurrencyPriceInfoResponse>(_httpClient, $"{panel}/price-info/{currencyId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdateAsync(int currencyId, CurrencyInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PutAsync<CurrencyInput, NoContent>(_httpClient, $"{panel}/{currencyId}", input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdatePriceAsync(int currencyId, CurrencyPriceInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PatchAsync<CurrencyPriceInput, NoContent>(_httpClient, $"{panel}/change-price/{currencyId}", input, cancellationToken);
            return result;
        }
    }
}