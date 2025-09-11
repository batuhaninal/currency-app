using API.Hubs.v1;
using Application.Abstractions.Notifiers;
using Application.Models.Events;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifiers
{
    public sealed class CurrencyNotifierService : ICurrencyNotifierService
    {
        private readonly IHubContext<CurrencyHub> _hubContext;

        public CurrencyNotifierService(IHubContext<CurrencyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyPriceAsync(PriceUpdatedEvent @event, CancellationToken cancellationToken = default) =>
            // await _hubContext.Clients.Group(string.Format("{0}-{1}", "currency", @event.CurrencyId.ToString()))
            //     .SendAsync("ReceivePrice", @event, cancellationToken: cancellationToken);
            await _hubContext.Clients.Group(string.Format("{0}-{1}", "currency", @event.CurrencyId.ToString()))
                .SendAsync("ReceivePrice", @event, cancellationToken: cancellationToken);
            // await _hubContext.Clients.All
            //     .SendAsync("ReceivePrice", @event, cancellationToken: cancellationToken);
    }
}