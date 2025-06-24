using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Queries.Assets.GetUserAssetInfo
{
    public sealed record GetUsersAssetInfoQuery : IQuery
    {
        public GetUsersAssetInfoQuery()
        {

        }

        public GetUsersAssetInfoQuery(int assetId)
        {
            AssetId = assetId;
        }

        [JsonIgnore]
        public int AssetId { get; set; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}