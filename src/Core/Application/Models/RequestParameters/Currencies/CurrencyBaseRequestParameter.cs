using Application.Models.RequestParameters.Commons;

namespace Application.Models.RequestParameters.Currencies
{
    public class CurrencyBaseRequestParameter : BaseRequestParameter
    {
        public CurrencyBaseRequestParameter()
        {

        }
        
        public int[]? CategoryId { get; set; }
    }
}