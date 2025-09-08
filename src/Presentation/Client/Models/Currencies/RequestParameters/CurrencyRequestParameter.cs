using Client.Models.Commons;

namespace Client.Models.Currencies.RequestParameters
{
    public sealed class CurrencyRequestParameter : BaseRequestParameter
    {
        public CurrencyRequestParameter()
        {

        }

        public int[]? CategoryId { get; set; }
        public decimal? MinPurchasePrice { get; set; }
        public decimal? MaxPurchasePrice { get; set; }
        public decimal? MinSalePrice { get; set; }
        public decimal? MaxSalePrice { get; set; }

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

            if (this.CategoryId != null && this.CategoryId.Any())
            {
                foreach (var categoryId in this.CategoryId)
                {
                    result.Add(new KeyValuePair<string, object>("categoryId", categoryId));
                }
            }

            if (this.MinPurchasePrice.HasValue && this.MinPurchasePrice.Value > 0)
                result.Add(new KeyValuePair<string, object>("minPurchasePrice", this.MinPurchasePrice.Value));

            if (this.MaxPurchasePrice.HasValue && this.MaxPurchasePrice.Value > 0)
                result.Add(new KeyValuePair<string, object>("maxPurchasePrice", this.MaxPurchasePrice.Value));

            if (this.MinSalePrice.HasValue && this.MinSalePrice.Value > 0)
                result.Add(new KeyValuePair<string, object>("minSalePrice", this.MinSalePrice.Value));

            if (this.MaxSalePrice.HasValue && this.MaxSalePrice.Value > 0)
                result.Add(new KeyValuePair<string, object>("maxSalePrice", this.MaxSalePrice.Value));

            return result.ToArray();
        }
    }
}