using Client.Models.Commons;
using Client.Models.Currencies;
using Client.Services.Commons;

namespace Client.Services.Currencies
{
    public sealed class CurrencyTagService : BaseService, ICurrencyTagService
    {
        private const string panel = "panel";
        private readonly HttpClient _httpClient;

        public CurrencyTagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<NoContent>> AddAsync(CurrencyTagInput input, CancellationToken cancellationToken = default)
        {
            var result = await this.PostAsync<CurrencyTagInput, NoContent>(_httpClient, panel, input, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> DeleteAsync(int currencyTagId, CancellationToken cancellationToken = default)
        {
            var result = await this.DeleteAsync<NoContent>(_httpClient, $"{panel}/{currencyTagId}", cancellationToken);
            return result;
        }

        public async Task<BaseResult<CurrencyTagInfoResponse>> InfoAsync(int currencyTagId, CancellationToken cancellationToken = default)
        {
            var result = await this.GetAsync<CurrencyTagInfoResponse>(_httpClient, $"{panel}/{currencyTagId}", null, cancellationToken);
            return result;
        }

        public async Task<BaseResult<NoContent>> UpdateAsync(int currencyTagId, CurrencyTagInput input, CancellationToken cancellationToken = default)
        {
            var result = await this.PutAsync<CurrencyTagInput, NoContent>(_httpClient, $"{panel}/{currencyTagId}", input, cancellationToken);
            return result;
        }
    }
}