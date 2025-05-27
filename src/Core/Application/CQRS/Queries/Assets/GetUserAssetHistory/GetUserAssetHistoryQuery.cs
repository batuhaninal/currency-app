using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Assets;

namespace Application.CQRS.Queries.Assets.GetUserAssetHistory
{
    public sealed class GetUserAssetHistoryQuery : AssetBaseRequestParameter, IQuery
    {
        public GetUserAssetHistoryQuery()
        {

        }
    }
}