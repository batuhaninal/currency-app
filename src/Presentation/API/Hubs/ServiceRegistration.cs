using API.Hubs.v1;

namespace API.Hubs
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication app)
        {
            app.MapHub<CurrencyHub>("/hubs/currencies");
        }
    }
}