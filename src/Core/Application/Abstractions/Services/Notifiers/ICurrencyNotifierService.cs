using Application.Models.Events;

namespace Application.Abstractions.Notifiers
{
    public interface ICurrencyNotifierService
    {
        Task NotifyPriceAsync(PriceUpdatedEvent @event, CancellationToken cancellationToken = default);
    }
}