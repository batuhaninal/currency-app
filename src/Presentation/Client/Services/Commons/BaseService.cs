using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Client.Models.Commons;

namespace Client.Services.Commons
{
    public abstract class BaseService
    {
        public virtual async Task<BaseResult<TResponse>> PostAsync<TRequest, TResponse>(HttpClient client, string url, TRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                url = NormalizeUrl(url);

                var response = await client.PostAsJsonAsync<TRequest>(url, request, cancellationToken);

                var result = await this.CheckResultAsync<TResponse>(response, cancellationToken);

                return result;
            }
            catch (System.Exception ex)
            {
                return new BaseResult<TResponse>(500, false, default, "Beklenmeyen Hata!");
            }
        }

        public virtual async Task<BaseResult<TResponse>> PutAsync<TRequest, TResponse>(HttpClient client, string url, TRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                url = NormalizeUrl(url);

                var response = await client.PutAsJsonAsync<TRequest>(url, request, cancellationToken);

                var result = await this.CheckResultAsync<TResponse>(response, cancellationToken);

                return result;
            }
            catch (System.Exception ex)
            {
                return new BaseResult<TResponse>(500, false, default, "Beklenmeyen Hata!");
            }
        }

        public virtual async Task<BaseResult<TResponse>> PatchAsync<TRequest, TResponse>(HttpClient client, string url, TRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                url = NormalizeUrl(url);

                var response = await client.PatchAsJsonAsync<TRequest>(url, request, cancellationToken);

                var result = await this.CheckResultAsync<TResponse>(response, cancellationToken);

                return result;
            }
            catch (System.Exception ex)
            {
                return new BaseResult<TResponse>(500, false, default, "Beklenmeyen Hata!");
            }
        }

        public virtual async Task<BaseResult<TResponse>> DeleteAsync<TResponse>(HttpClient client, string url, CancellationToken cancellationToken = default)
        {
            try
            {
                url = NormalizeUrl(url, null);

                var response = await client.DeleteAsync(url, cancellationToken: cancellationToken);

                var result = await this.CheckResultAsync<TResponse>(response, cancellationToken);

                return result;
            }
            catch (System.Exception ex)
            {
                return new BaseResult<TResponse>(500, false, default, "Beklenmeyen Hata!");
            }
        }

        public virtual async Task<BaseResult<TResponse>> GetAsync<TResponse>(HttpClient client, string url, KeyValuePair<string, object>[]? parameters = null, CancellationToken cancellationToken = default)
        {
            try
            {
                url = NormalizeUrl(url, parameters);

                var response = await client.GetAsync(url, cancellationToken: cancellationToken);

                var result = await this.CheckResultAsync<TResponse>(response, cancellationToken);

                return result;
            }
            catch (System.Exception ex)
            {
                return new BaseResult<TResponse>(500, false, default, "Beklenmeyen Hata!");
            }
        }

        private async Task<BaseResult<TResponse>> CheckResultAsync<TResponse>(HttpResponseMessage? response, CancellationToken cancellationToken = default)
        {
            if (response == null)
                return new BaseResult<TResponse>(500, false, default, "Yanıt alınamadı.");

            try
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var result = JsonSerializer.Deserialize<BaseResult<TResponse>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (result is not null)
                {
                    return result.StatusCode switch
                    {
                        (int)HttpStatusCode.Unauthorized => new BaseResult<TResponse>(result.StatusCode, false, default, "Lütfen giriş yapınız!"),
                        (int)HttpStatusCode.Forbidden => new BaseResult<TResponse>(result.StatusCode, false, default, "Bu işleme yetkiniz bulunmamaktadır!"),
                        // (int)HttpStatusCode.NoContent => new BaseResult<TResponse>(result.StatusCode, true),
                        // (int)HttpStatusCode.NonAuthoritativeInformation => new BaseResult<TResponse>(result.StatusCode, true),
                        _ => result
                    };
                }

                return new BaseResult<TResponse>((int)response.StatusCode, false, default, "Sunucudan beklenen formatta cevap alınamadı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}, Inner Ex: {ex.InnerException}");
                return new BaseResult<TResponse>((int)response.StatusCode, false, default, "Sunucu hatalı yanıt verdi.");
            }
        }

        private string NormalizeUrl(string url, KeyValuePair<string, object>[]? queries = null)
        {
            url = url.ToLower();

            if (queries != null && queries.Length > 0)
            {
                var paramList = queries.Select(x => $"{x.Key.ToLower()}={Uri.EscapeDataString(x.Value?.ToString() ?? string.Empty)}");

                return $"{url}?{string.Join("&", paramList)}";
            }

            return url;
        }
    }
}