using Client.Models.Commons;
using Client.Models.Currencies;
using Client.Models.Currencies.RequestParameters;
using Client.Services.Commons;

namespace Client.Services.Currencies
{
    public sealed class CurrencyService : BaseService, ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<NoContent>> AddAsync(CurrencyInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PostAsync<CurrencyInput, NoContent>(_httpClient, "", input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> ChangeStatusAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PatchAsync<NoContent, NoContent>(_httpClient, $"status/{currencyId}", new(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> DeleteAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.DeleteAsync<NoContent>(_httpClient, $"{currencyId}", cancellationToken);
            return result;
        }

        public async Task<BaseResult<CurrencyInfoResponse>> InfoAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<CurrencyInfoResponse> result = await this.GetAsync<CurrencyInfoResponse>(_httpClient, $"{currencyId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<PaginationResult<CurrencyItemResponse>>> ListAsync(CurrencyRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<PaginationResult<CurrencyItemResponse>> result = await this.GetAsync<PaginationResult<CurrencyItemResponse>>(_httpClient, "", parameter.ToQueryString(), cancellationToken);
            return result;
        }

        public async Task<BaseResult<CurrencyPriceInfoResponse>> PriceInfoAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            BaseResult<CurrencyPriceInfoResponse> result = await this.GetAsync<CurrencyPriceInfoResponse>(_httpClient, $"price-info/{currencyId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdateAsync(int currencyId, CurrencyInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PutAsync<CurrencyInput, NoContent>(_httpClient, $"{currencyId}", input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdatePriceAsync(int currencyId, CurrencyPriceInput input, CancellationToken cancellationToken = default)
        {
            BaseResult<NoContent> result = await this.PatchAsync<CurrencyPriceInput, NoContent>(_httpClient, $"change-price/{currencyId}", input, cancellationToken);
            return result;
        }
    }
}