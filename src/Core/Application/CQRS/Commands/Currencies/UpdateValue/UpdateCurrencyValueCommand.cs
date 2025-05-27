using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Commands.Currencies.UpdateValue
{
    public sealed record UpdateCurrencyValueCommand : ICommand
    {
        public UpdateCurrencyValueCommand()
        {

        }

        public UpdateCurrencyValueCommand(int currencyId, decimal purchasePrice, decimal salePrice)
        {
            CurrencyId = currencyId;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
        }

        [JsonIgnore]
        public int CurrencyId { get; set; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
    }
}