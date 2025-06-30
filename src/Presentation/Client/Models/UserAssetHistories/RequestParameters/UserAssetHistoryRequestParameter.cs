using Client.Models.Commons;

namespace Client.Models.UserAssetHistories.RequestParameters
{
    public sealed class UserAssetHistoryRequestParameter : BaseRequestParameter
    {
        public UserAssetHistoryRequestParameter()
        {

        }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        
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

            if (this.StartDate != null)
                result.Add(new KeyValuePair<string, object>("startDate", this.StartDate.Value));

            if (this.EndDate != null)
                result.Add(new KeyValuePair<string, object>("endDate", this.EndDate.Value));    

            return result.ToArray();
        }
    }
}