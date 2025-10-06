using System.Security.Claims;
using System.Threading.RateLimiting;
using Application.Models.Constants.Settings;
using Application.Models.DTOs.Commons.Results;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Middlewares.Extensions
{
    public static class RateLimitingExtension
    {
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, token) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter = $"{retryAfter.TotalSeconds}";

                        await context.HttpContext.Response.WriteAsJsonAsync<ResultDto>(new ResultDto(StatusCodes.Status429TooManyRequests, false, null, $"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds."));
                    }
                };

                options.AddFixedWindowLimiter(SettingConstant.FixedRateLimiting, cfg =>
                {
                    cfg.PermitLimit = 45;
                    cfg.Window = TimeSpan.FromMinutes(1);
                });

                options.AddPolicy(SettingConstant.PerUserRateLimiting, httpContext =>
                {
                    string? userId = httpContext.User.FindFirstValue("sub");

                    if (!string.IsNullOrEmpty(userId))
                    {
                        Console.WriteLine("UserId = " + userId);
                        return RateLimitPartition.GetTokenBucketLimiter(userId,
                            _ => new TokenBucketRateLimiterOptions
                            {
                                TokenLimit = 30, // Maksimum 30 isteklik burst (ani artış)
                                TokensPerPeriod = 10, // Her periyotta 10 token yenilenir
                                ReplenishmentPeriod = TimeSpan.FromMinutes(1), // Her 1 dakikada yenileme
                                AutoReplenishment = true, // Otomatik yenilensin
                                // QueueLimit = 5, // 5 istek beklemeye alınabilir
                                // QueueProcessingOrder = QueueProcessingOrder.OldestFirst // FIFO
                            }
                        );
                    }

                    Console.WriteLine("UserId okunamadi");

                    return RateLimitPartition.GetFixedWindowLimiter(
                        SettingConstant.AnonymousRateLimiting,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 45, // 1 dakikada 45 istek
                            Window = TimeSpan.FromMinutes(1), // 1 dakikalık pencere
                            // QueueLimit = 0, // Kuyruk yok, doğrudan reddet
                            AutoReplenishment = true
                        });
                });

                options.AddPolicy(SettingConstant.RichRateLimiting, httpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        SettingConstant.RichRateLimiting,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100, // 1 dakikada 100 istek
                            Window = TimeSpan.FromMinutes(1), // 1 dakikalık pencere
                            // QueueLimit = 0, // Kuyruk yok, doğrudan reddet
                            AutoReplenishment = true
                        });
                });

                options.AddPolicy(SettingConstant.AnonymousRateLimiting, HttpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        SettingConstant.AnonymousRateLimiting,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 45, // 1 dakikada 45 istek
                            Window = TimeSpan.FromMinutes(1), // 1 dakikalık pencere
                            // QueueLimit = 0, // Kuyruk yok, doğrudan reddet
                            AutoReplenishment = true
                        });
                });
            });
        }
    }
}