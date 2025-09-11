namespace Client.Models.UserCurrencyFollows.RequestParameters
{
    public sealed class BroadcastParameters
    {
        public int? Take { get; set; }
        public bool? IsBroadcast { get; set; }
        public bool? All { get; set; }

        internal KeyValuePair<string, object>[] ToQueryString()
        {
            List<KeyValuePair<string, object>> result = new();

            if (this.Take != null)
                result.Add(new KeyValuePair<string, object>("take", this.Take.Value));

            if (this.IsBroadcast != null)
                result.Add(new KeyValuePair<string, object>("isBroadcast", this.IsBroadcast.Value));

            if(this.All != null)
                result.Add(new KeyValuePair<string, object>("all", this.All.Value));

            return result.ToArray();
        }
    }
}