using System.Net.Http.Json;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.Externals;
using Microsoft.Extensions.Logging;

namespace Adapter.Services.Externals
{
    public sealed class TradingViewService : ITradingViewService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TradingViewService> _logger;

        public TradingViewService(HttpClient httpClient, ILogger<TradingViewService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TradingViewResponse> FetchData(string currency, string reference = "TRY", CancellationToken cancellationToken = default)
        {
            try
            {
                TradingViewRequestModel request = new TradingViewRequestModel();
                request.SetCurrency(currency, reference);

                foreach (var item in request.Filter)
                {
                    Console.WriteLine(item.Right);
                }

                var response = await _httpClient.PostAsJsonAsync("forex/scan", request);

                if (response.IsSuccessStatusCode)
                {
                    TradingViewResponse? responseModel = await response.Content.ReadFromJsonAsync<TradingViewResponse>();
                    if (responseModel != null)
                        if (responseModel.Data != null)
                        {
                            foreach (var item in responseModel.Data)
                            {
                                item.Date = DateTime.UtcNow;
                                item.Currency = currency;
                            }
                        }
                    return responseModel ?? new TradingViewResponse(0, null);
                }

                _logger.LogError("{StatusCode} {Response}", response.StatusCode, await response.Content.ReadAsStringAsync());

                return new TradingViewResponse(0, null);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("{Function} exception: {Exception} at: {Now}", typeof(TradingViewService).Name, ex, DateTime.UtcNow);
                return new TradingViewResponse(0, null);
            }
        }

        public async Task<List<TradingViewResponse>> FetchData(int delay, string reference, CancellationToken cancellationToken = default, params string[] currencies)
        {
            List<TradingViewResponse> result = new List<TradingViewResponse>();

            foreach (var currency in currencies)
            {
                var response = await this.FetchData(currency, reference, cancellationToken);
                if (response.Data != null)
                    result.Add(response);

                await Task.Delay(delay, cancellationToken);
            }

            return result;
        }
    }
}