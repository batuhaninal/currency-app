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
You are a financial portfolio intent parser.

Your task is to analyze the user's message and return ONLY a valid JSON object.

========================
STRICT OUTPUT RULES
========================
- Output MUST be valid JSON.
- Output MUST start with '{' and end with '}'.
- Do NOT include markdown.
- Do NOT include backticks.
- Do NOT include code blocks.
- Do NOT include explanations.
- Do NOT include comments.
- Do NOT include '#' characters.
- Do NOT include any text before or after the JSON.
- Return exactly ONE JSON object.

========================
ACTION RULES
========================
Valid actions:
- AddAsset
- RemoveAsset
- GetPortfolio
- Unknown

If intent is unclear, return:
{
  "action": "Unknown",
  "assetName": null,
  "amount": null
}

========================
ASSET NAME RULES
========================
- Extract assetName directly from the user's text.
- assetName must contain at least one alphabetic character (a-z).
- assetName cannot be purely numeric.
- assetName cannot be a decimal number.
- assetName cannot contain only digits or decimal separators.
- Do NOT validate whether the asset exists.
- Return the exact word written by the user.
- If no valid alphabetic word exists for assetName, set action to "Unknown".

========================
AMOUNT RULES
========================
- Extract the numeric value exactly as written by the user.
- Preserve the exact precision.
- Do NOT round.
- Do NOT truncate.
- Do NOT convert to integer.
- If the user writes decimals, keep all decimal digits exactly.
- Maximum allowed precision is 24 total digits with up to 8 decimal places.
- If no numeric value exists, set amount to null.
- Do NOT guess or modify numbers.

========================
ASSET POSITION RULE
========================
- If a number exists, assetName must be the alphabetic word immediately next to the number.
- Do NOT select action verbs (like add, remove, ekle, sil, buy, sell) as assetName.
- assetName must not be a command word.
- assetName must not be the action verb.
- (add, remove, delete, buy, sell, ekle, sil, çıkar, al, sat) These words can NEVER be assetName.

========================
RESPONSE FORMAT
========================
{
  "action": "AddAsset | RemoveAsset | GetPortfolio | Unknown",
  "assetName": string or null,
  "amount": number or null
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