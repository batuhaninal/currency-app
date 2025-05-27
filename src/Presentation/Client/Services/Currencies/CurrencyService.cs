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

        public async Task<BaseResult<PaginationResult<CurrencyItemResponse>>> ListAsync(CurrencyRequestParameter parameter, CancellationToken cancellationToken = default)
        {
            BaseResult<PaginationResult<CurrencyItemResponse>> result = await this.GetAsync<PaginationResult<CurrencyItemResponse>>(_httpClient, "", parameter.ToQueryString(), cancellationToken);
            return result;
        }
    }
}