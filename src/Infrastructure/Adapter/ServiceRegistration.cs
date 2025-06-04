using Adapter.Services.BackgroundServices;
using Adapter.Services.Externals;
using Adapter.Services.Security;
using Adapter.Services.Tokens;
using Application.Abstractions.Commons.Security;
using Application.Abstractions.Commons.Tokens;
using Application.Abstractions.Services.Externals;
using Application.Models.Constants.APIs;
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

            services.AddScoped<IUserTokenService, UserTokenService>();

            services.AddHttpClient<ITradingViewService, TradingViewService>(cfg =>
            {
                cfg.BaseAddress = new Uri(ExternalApiUrls.TradingView);
            });

            services.AddHttpClient<IWebScrappingService, WebScrappingService>();

            services.AddHostedService<HourlyCurrencyBackgroundService>();
            services.AddHostedService<HourlyScrapperBackgroundService>();
        }
    }
}
