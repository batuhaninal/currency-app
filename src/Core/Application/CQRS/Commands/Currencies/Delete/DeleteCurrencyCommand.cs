using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Commands.Currencies.Delete
{
    public sealed record DeleteCurrencyCommand : ICommand
    {
        public DeleteCurrencyCommand()
        {

        }

        public DeleteCurrencyCommand(int currencyId)
        {
            CurrencyId = currencyId;
        }
        public int CurrencyId { get; init; }
    }
}