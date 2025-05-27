using Application.Models.RequestParameters.Commons;

namespace Application.Models.RequestParameters.Assets
{
    public class AssetBaseRequestParameter : BaseRequestParameter
    {
        public AssetBaseRequestParameter()
        {

        }
        
        public int[]? CurrencyId { get; set; }
    }
}