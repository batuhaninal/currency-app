using Application.Models.RequestParameters.Commons;

namespace Application.Models.RequestParameters.Currencies
{
    public class CurrencyBaseRequestParameter : BaseRequestParameter
    {
        public CurrencyBaseRequestParameter()
        {

        }

        public int[]? CategoryId { get; set; }
        public decimal? MinPurchasePrice { get; set; }
        public decimal? MaxPurchasePrice { get; set; }
        public decimal? MinSalePrice { get; set; }
        public decimal? MaxSalePrice { get; set; }
    }
}