using System.Text.Json.Serialization;
using Application.Models.Enums;

namespace Application.Models.DTOs.AIs
{
    public class AiIntent
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AiAction Action { get; set; }
        public string? AssetName { get; set; }
        public decimal? Amount { get; set; }
    }
}