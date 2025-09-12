using Client.Models.Commons;

namespace Client.Models.UserCurrencyFollows.RequestParameters
{
    public class UserCurrencyFollowRequestParameter : BaseRequestParameter
    {
        public UserCurrencyFollowRequestParameter()
        {

        }

        public int[]? CurrencyIds { get; set; }
        public DateTime? MinUpdatedDate { get; set; }
        public DateTime? MaxUpdatedDate { get; set; }

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

            if (this.MinUpdatedDate.HasValue)
                result.Add(new KeyValuePair<string, object>("minUpdatedDate", this.MinUpdatedDate.Value));

            if (this.MaxUpdatedDate.HasValue)
                result.Add(new KeyValuePair<string, object>("maxUpdatedDate", this.MaxUpdatedDate.Value));

            if (this.CurrencyIds is not null && this.CurrencyIds.Any())
            {
                foreach (var currencyId in this.CurrencyIds)
                {
                    result.Add(new KeyValuePair<string, object>("currencyIds", currencyId));
                }
            }

            return result.ToArray();
        }
    }
}