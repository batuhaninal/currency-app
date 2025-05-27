namespace Client.Models.Commons
{
    public sealed record SortModel
    {
        public SortModel()
        {
            
        }

        public SortModel(string field)
        {
            Field = field;
        }

        public string Field { get; init; } = null!;
    }
}