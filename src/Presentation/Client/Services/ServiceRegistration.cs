using Client.Middlewares;
using Client.Models.Constants;
using Client.Services.Assets;
using Client.Services.Auth;
using Client.Services.Categories;
using Client.Services.Currencies;
using Client.Services.Tools;
using Client.Services.Users;

namespace Client.Services
{
    public static class ServiceRegistration
    {
        public static void BindClientService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<TokenMiddleware>();

            services.AddHttpClient<IAuthService, AuthService>(opt =>
            {
                opt.BaseAddress = new Uri($"{AppConstants.APIURL}/auth/");
            });

            services.AddHttpClient<ICategoryService, CategoryService>(opt =>
            {
                opt.BaseAddress = new Uri($"{AppConstants.APIURL}/panel/categories/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<ICurrencyService, CurrencyService>(opt =>
            {
                opt.BaseAddress = new Uri($"{AppConstants.APIURL}/panel/currencies/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<IToolService, ToolService>(opt =>
            {
                opt.BaseAddress = new Uri($"{AppConstants.APIURL}/tools/");
            });

            services.AddHttpClient<IAssetService, AssetService>(opt =>
            {
                opt.BaseAddress = new Uri($"{AppConstants.APIURL}/assets/");
            }).AddHttpMessageHandler<TokenMiddleware>();

            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri($"{AppConstants.APIURL}/users/");
            }).AddHttpMessageHandler<TokenMiddleware>();
        }
    }
}