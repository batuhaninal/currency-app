using Client.Models.Commons;

namespace Client.Models.Categories.RequestParameters
{
    public class CategoryBaseRequestParameter : BaseRequestParameter
    {
        public CategoryBaseRequestParameter()
        {

        }

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

            return result.ToArray();
        }
    }
}