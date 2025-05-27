namespace Client.Models.Commons
{
    public sealed class SearchModel
    {
        public SearchModel()
        {
            Condition = string.Empty;
        }

        
        public SearchModel(string condition)
        {
            Condition = condition;
        }
        
        public string Condition { get; init; } = string.Empty;
    }
}