namespace Client.Models.Currencies.RequestParameters
{
    public sealed class CurrencyHistoryRequestParameter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        internal KeyValuePair<string, object>[] ToQueryString()
        {
            List<KeyValuePair<string, object>> result = new();

            if (this.StartDate != null)
                result.Add(new KeyValuePair<string, object>("startDate", this.StartDate.Value));

            if (this.EndDate != null)
                result.Add(new KeyValuePair<string, object>("endDate", this.EndDate.Value));

            return result.ToArray();
        }
    }
}