using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Abstractions.Services.Externals;
using Application.Models.DTOs.AIs;
using Application.Models.RequestParameters.AIs;

namespace Adapter.Services.Externals
{
    public sealed class OllamaAIService : IAiService
    {
        private readonly HttpClient _httpClient;

        private readonly string _sysPrompt;
        public OllamaAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _sysPrompt = """
        Sen bir yatırım portföyü intent parserısın.

        Kullanıcı mesajını analiz et ve SADECE aşağıdaki JSON formatında cevap ver.

        Kurallar:
        - Sadece JSON dön.
        - Markdown kullanma.
        - Kod bloğu kullanma.
        - Açıklama yazma.
        - JSON dışı hiçbir karakter üretme.

        Format:
        {
          "action": "AddAsset | RemoveAsset | GetPortfolio | Unknown",
          "assetName": string | null,
          "amount": number | null
        }
        """;
        }

        public async Task<AiIntent> ParseAsync(string userMessage, CancellationToken cancellationToken = default)
        {
            var fullPrompt = $"{_sysPrompt}\n\nKullanıcı Mesajı: {userMessage}";

            var request = new OllamaRequest
            {
                Prompt = fullPrompt
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/generate",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken);

            return SafeParse(result!.Response);
        }

        private AiIntent SafeParse(string raw)
        {
            raw = raw.Trim();

            Console.WriteLine(raw);

            var start = raw.IndexOf('{');
            var end = raw.LastIndexOf('}');

            if (start == -1 || end == -1)
                throw new Exception("Invalid AI response");

            var json = raw.Substring(start, end - start + 1);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new JsonStringEnumConverter() }
            };

            return JsonSerializer.Deserialize<AiIntent>(json, options)!;
        }

    }
}