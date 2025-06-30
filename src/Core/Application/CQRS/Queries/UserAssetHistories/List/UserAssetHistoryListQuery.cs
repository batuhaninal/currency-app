using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.UserAssetHistories;

namespace Application.CQRS.Queries.UserAssetHistories.List
{
    public sealed class UserAssetHistoryListQuery : UserAssetHistoryBaseRequestParameter, IQuery
    {
        public UserAssetHistoryListQuery()
        {

        }
    }
}