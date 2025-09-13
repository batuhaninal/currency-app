using System.Text.Json;
using Client.Models.Commons;

public static class CookieExtensions
{
    public static void SetObject<T>(this IResponseCookies cookies, string key, T value, int? expireMinutes = null)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = expireMinutes.HasValue ? DateTimeOffset.Now.AddMinutes(expireMinutes.Value) : DateTimeOffset.Now.AddDays(1)
        };

        cookies.Append(key, JsonSerializer.Serialize(value), options);
    }

    public static List<BreadcrumbItem> GetBreadcrumbs(this IRequestCookieCollection cookies, string key)
    {
        if (cookies.TryGetValue(key, out var value))
        {
            try
            {
                return JsonSerializer.Deserialize<List<BreadcrumbItem>>(value) ?? new List<BreadcrumbItem>();
            }
            catch
            {
                return new List<BreadcrumbItem>();
            }
        }
        return new List<BreadcrumbItem>();
    }
}
