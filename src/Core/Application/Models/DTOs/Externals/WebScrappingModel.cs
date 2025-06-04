namespace Application.Models.DTOs.Externals
{
    public sealed record XAUData
    {
        public XAUData()
        {
            
        }

        public XAUData(string key, string attr, decimal value)
        {
            Key = key;
            Attr = attr;
            Value = value;
        }

        public string Key { get; init; } = null!;
        public string Attr { get; init; } = null!;
        public decimal Value { get; init; }
    }
}