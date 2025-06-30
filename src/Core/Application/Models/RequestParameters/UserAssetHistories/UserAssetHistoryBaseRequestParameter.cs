using Application.Models.RequestParameters.Commons;

namespace Application.Models.RequestParameters.UserAssetHistories
{
    public class UserAssetHistoryBaseRequestParameter : BaseRequestParameter
    {
        public UserAssetHistoryBaseRequestParameter()
        {

        }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    } 
}