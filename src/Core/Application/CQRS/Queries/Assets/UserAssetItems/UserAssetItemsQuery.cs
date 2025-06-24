using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Assets;

namespace Application.CQRS.Queries.Assets.UserAssetItems
{
    public sealed class UserAssetItemsQuery : AssetBaseRequestParameter, IQuery
    {
        public UserAssetItemsQuery()
        {

        }
    }
}