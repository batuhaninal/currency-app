using Client.Middlewares;
using Client.Models.Commons;
using Client.Models.Constants;
using Client.Services.Assets;
using Client.Services.Auth;
using Client.Services.Categories;
using Client.Services.Currencies;
using Client.Services.Tools;
using Client.Services.UserAssetHistories;
using Client.Services.UserCurrencyFollows;
using Client.Services.Users;

namespace Client.Services
{
    public static class ServiceRegistration
    {
        public static void BindClientService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<TokenMiddleware>();
            services.AddScoped<ClientCredentialMiddleware>();

            services.Configure<ServiceApiSetting>(configuration.GetSection("ServiceApiSettings"));

            var serviceApiSetting = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSetting>();

            services.AddHttpClient<IAuthService, AuthService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/auth/");
            }).AddHttpMessageHandler<ClientCredentialMiddleware>();

            services.AddHttpClient<ICategoryService, CategoryService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/categories/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<ICurrencyService, CurrencyService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/currencies/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<ICurrencyTagService, CurrencyTagService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/currencies/tags/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<IToolService, ToolService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/tools/");
            }).AddHttpMessageHandler<ClientCredentialMiddleware>();

            services.AddHttpClient<IAssetService, AssetService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/assets/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/users/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<IUserAssetHistoryService, UserAssetHistoryService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/user-asset-histories/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<IUserCurrencyFollowService, UserCurrencyFollowService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSetting.ApiUrl}/api/user-currency-follows/");
            }).AddHttpMessageHandler<TokenMiddleware>();
        }
    }
}