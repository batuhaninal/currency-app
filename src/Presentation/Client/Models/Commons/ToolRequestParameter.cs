namespace Client.Models.Commons
{
    public class ToolRequestParameter
    {
        private string? _condition;
        public ToolRequestParameter()
        {

        }
        public string? Condition
        {
            get => _condition;
            set => _condition = value?.TrimStart().TrimEnd().ToLower();
        }
        public string? OrderBy { get; set; }
        
        internal KeyValuePair<string, object>[] ToQueryString()
        {
            List<KeyValuePair<string, object>> result = new();

            if (this.Condition != null)
                result.Add(new KeyValuePair<string, object>("condition", this.Condition));

            if (this.OrderBy != null)
                result.Add(new KeyValuePair<string, object>("orderBy", this.OrderBy));

            return result.ToArray();
        }
    }
}