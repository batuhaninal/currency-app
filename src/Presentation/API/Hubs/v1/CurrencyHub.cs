using Microsoft.AspNetCore.SignalR;

namespace API.Hubs.v1
{
    public class CurrencyHub : Hub
    {
        public async Task SubscribeToCurrency(int currencyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, string.Format("{0}-{1}", "currency", currencyId.ToString()));
        }

        public async Task UnsubscribeFromCurrency(int currencyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, string.Format("{0}-{1}", "currency", currencyId.ToString()));
        }
    }
}