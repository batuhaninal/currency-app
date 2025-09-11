using Application.Abstractions.Notifiers;

namespace API.Notifiers
{
    public static class NotifierRegistration
    {
        public static void BindNotifiers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICurrencyNotifierService, CurrencyNotifierService>();
        }
    }
}