using System.Text.Json.Serialization;

namespace Application.Models.DTOs.Externals
{
    public sealed record TradingViewRequestModel
    {

        public TradingViewRequestModel()
        {
            Filter = new List<TradingViewFilter>()
            {
                new TradingViewFilter
                {
                    Left = "name,description",
                    Operation = "match",
                    Right = "USDTRY"
                }
            };
            Options = new TradingViewOptions();
            Markets = new string[] { "forex" };
            Columns = new string[] { "close" };
            Range = new int[] { 0, 1 };
        }

        public TradingViewRequestModel(List<TradingViewFilter> filter, TradingViewOptions options, string[] markets, string[] columns, int[] range)
        {
            Filter = filter;
            Options = options;
            Markets = markets;
            Columns = columns;
            Range = range;
        }

        [JsonPropertyName("filter")]
        public List<TradingViewFilter> Filter { get; init; } = null!;

        [JsonPropertyName("options")]
        public TradingViewOptions Options { get; init; } = null!;

        [JsonPropertyName("markets")]
        public string[] Markets { get; init; } = new string[] { "forex" };

        [JsonPropertyName("columns")]
        public string[] Columns { get; init; } = new string[] { "close" };

        [JsonPropertyName("range")]
        public int[] Range { get; init; } = new int[] { 0, 1 };

        public void SetCurrency(string title, string reference = "TRY")
        {
            foreach (var item in Filter)
            {
                item.Right = title + reference;
            }
        }
    }

    public record TradingViewFilter
    {
        public TradingViewFilter()
        {
            Left = "name,description";
            Operation = "match";
            Right = "USDTRY";
        }

        public TradingViewFilter(string left, string operation, string right, string reference = "TRY")
        {
            Left = left;
            Operation = operation;
            Right = right + reference;
        }

        [JsonPropertyName("left")]
        public string Left { get; init; } = "name,description";

        [JsonPropertyName("operation")]
        public string Operation { get; init; } = "match";

        [JsonPropertyName("right")]
        public string Right { get; internal set; } = "USDTRY";
    }

    public record TradingViewOptions
    {
        public TradingViewOptions()
        {
            Lang = "tr";
        }
        public TradingViewOptions(string lang)
        {
            Lang = lang;
        }

        [JsonPropertyName("lang")]
        public string Lang { get; init; } = "tr";
    }

    public sealed record TradingViewResponse
    {
        public TradingViewResponse()
        {

        }

        public TradingViewResponse(int totalCount, List<TradingViewResponseData>? data)
        {
            TotalCount = totalCount;
            Data = data;
        }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; init; }

        [JsonPropertyName("data")]
        public List<TradingViewResponseData>? Data { get; init; }
    }

    public sealed record TradingViewResponseData
    {
        public TradingViewResponseData()
        {

        }

        public TradingViewResponseData(string? s, decimal[]? d)
        {
            S = s;
            D = d;
        }

        [JsonPropertyName("s")]
        public string? S { get; init; }

        [JsonPropertyName("d")]
        public decimal[]? D { get; init; }

        [JsonIgnore]
        public DateTime? Date { get; set; }
        [JsonIgnore]
        public string? Currency { get; set; }
    }
}