using System.Globalization;
using Application.Abstractions.Services.Externals;
using Application.Models.Constants.APIs;
using Application.Models.DTOs.Externals;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Adapter.Services.Externals
{
    public sealed class WebScrappingService : IWebScrappingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WebScrappingService> _logger;

        public WebScrappingService(HttpClient httpClient, ILogger<WebScrappingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<XAUData>> FetchDovizcomXAUDataAsync(string[] dataSocketKeys, CancellationToken cancellationToken = default)
        {
            var nodes = await this.FetchData(ExternalApiUrls.DovizComXAU, "//td[@data-socket-key and @data-socket-attr]");
            if (nodes == null)
            {
                _logger.LogError($"{0} service error: No data found when fetching from: {1} at {2}", typeof(WebScrappingService).Name, ExternalApiUrls.DovizComXAU, DateTime.UtcNow);
                return new();
            }

            var allNodes = nodes.Select(x =>
            {
                var key = x.GetAttributeValue("data-socket-key", string.Empty);
                var attr = x.GetAttributeValue("data-socket-attr", string.Empty);
                var rawValue = x.InnerText?.Trim() ?? "0";

                // Temizleme işlemi: $ işaretini, binlik noktaları sil, ondalık virgülü noktaya çevir
                var cleanedValue = rawValue
                    .Replace("$", "")
                    .Replace(".", "")
                    .Replace(",", ".");

                // Güvenli parse
                decimal.TryParse(cleanedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue);

                return new XAUData
                {
                    Key = key,
                    Attr = attr,
                    Value = parsedValue
                };
            })
            .ToList();

            return allNodes.Where(x => dataSocketKeys.Contains(x.Key)).ToList();
        }

        private async Task<HtmlNodeCollection?> FetchData(string url, string xpath, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(url, cancellationToken);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response);

                var nodes = htmlDoc.DocumentNode.SelectNodes(xpath);

                return nodes;
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("İstek iptal edildi.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Altın verilerini çekerken hata oluştu.");
                return null;
            }
        }
    }
}