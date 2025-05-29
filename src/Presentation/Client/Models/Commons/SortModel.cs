namespace Client.Models.Commons
{
    public sealed record SortModel
    {
        public SortModel()
        {

        }

        public SortModel(string field, bool disable = false)
        {
            Field = field;
            Disable = disable;
        }

        public string Field { get; init; } = null!;
        public bool Disable { get; init; }
    }
}