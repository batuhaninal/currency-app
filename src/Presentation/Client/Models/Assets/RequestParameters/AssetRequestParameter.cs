using Client.Models.Commons;

namespace Client.Models.Assets.RequestParameters
{
    public class AssetRequestParameter : BaseRequestParameter
    {
        public AssetRequestParameter()
        {

        }
        public int[]? CurrencyId { get; set; }
        
        internal KeyValuePair<string, object>[] ToQueryString()
        {
            List<KeyValuePair<string, object>> result = new();

            if (this.PageIndex != null)
                result.Add(new KeyValuePair<string, object>("pageIndex", this.PageIndex.Value));

            if (this.PageSize != null)
                result.Add(new KeyValuePair<string, object>("pageSize", this.PageSize.Value));

            if (this.Condition != null)
                result.Add(new KeyValuePair<string, object>("condition", this.Condition));

            if (this.OrderBy != null)
                result.Add(new KeyValuePair<string, object>("orderBy", this.OrderBy));

            if (this.IsActive != null)
                result.Add(new KeyValuePair<string, object>("isActive", this.IsActive.Value));

            if (this.CurrencyId != null && this.CurrencyId.Any())
            {
                foreach (var currencyId in this.CurrencyId)
                {
                    result.Add(new KeyValuePair<string, object>("currencyId", currencyId));
                }
            }

            return result.ToArray();
        }
    }
}