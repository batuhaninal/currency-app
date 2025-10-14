using Adapter.Services.BackgroundServices;
using Adapter.Services.Caching;
using Adapter.Services.Externals;
using Adapter.Services.Logger;
using Adapter.Services.Security;
using Adapter.Services.Tokens;
using Application.Abstractions.Commons.Caching;
using Application.Abstractions.Commons.Logger;
using Application.Abstractions.Commons.Security;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Services.Externals;
using Application.Models.Constants.APIs;
using Application.Models.Constants.Settings;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter
{
    public static class ServiceRegistration
    {
        public static void BindAdapterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<IHashingService, HashingService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));

            services.Configure<ServiceApiSetting>(configuration.GetSection("ServiceApiSettings"));

            var serviceApiSetting = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSetting>();

            ExternalApiUrls.DovizComXAU = serviceApiSetting.DovizCom.Path;

            services.AddScoped<IUserTokenService, UserTokenService>();
            services.AddSingleton<ICacheService>(serviceProvider =>
            {
                var redisOptions = new RedisCacheOptions
                {
                    ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                    {
                        EndPoints = { { configuration["Redis:Host"]!, int.Parse(configuration["Redis:Port"]!.ToString()) } },
                        Password = configuration["Redis:Password"],
                        DefaultDatabase = int.Parse(configuration["Redis:DbIndex"]!),
                        AbortOnConnectFail = false
                    }
                };

                return new CacheService(redisOptions.ConfigurationOptions);
            });

            services.AddHttpClient<ITradingViewService, TradingViewService>(cfg =>
            {
                cfg.BaseAddress = new Uri(serviceApiSetting.TradingView.Path);
            });

            services.AddHttpClient<IWebScrappingService, WebScrappingService>();

            services.AddHostedService<HourlyCurrencyBackgroundService>();
            services.AddHostedService<HourlyScrapperBackgroundService>();
            services.AddHostedService<UserAssetHistoryBackgroundService>();
        }
    }
}
