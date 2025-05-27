using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Commands.Currencies.ChangeStatus
{
    public sealed record ChangeCurrencyStatusCommand : ICommand
    {
        public ChangeCurrencyStatusCommand()
        {

        }

        public ChangeCurrencyStatusCommand(int currencyId)
        {
            CurrencyId = currencyId;
        }

        public int CurrencyId { get; init; }
    }
}