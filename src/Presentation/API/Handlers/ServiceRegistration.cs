using API.Handlers.v1;
using Application.Abstractions.Handlers;

namespace API.Handlers
{
    public static class HandlerRegistration
    {
        public static void BindHandlerService(this IServiceCollection services)
        {
            services.AddScoped<IAuthHandler, AuthHandler>();
            services.AddScoped<IUserHandler, UserHandler>();
            services.AddScoped<IAssetHandler, AssetHandler>();
            services.AddScoped<ICurrencyHandler, CurrencyHandler>();
            services.AddScoped<ICategoryHandler, CategoryHandler>();
            services.AddScoped<IToolHandler, ToolHandler>();
            services.AddScoped<IUserAssetHistoryHandler, UserAssetHistoryHandler>();
            services.AddScoped<IUserCurrencyFollowHandler, UserCurrencyFollowHandler>();
            services.AddScoped<IAIHandler, AIHandler>();
        }
    }
}