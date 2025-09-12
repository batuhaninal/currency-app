using Application.Models.RequestParameters.Commons;

namespace Application.Models.RequestParameters.UserCufrrencyFollows
{
    public class UserCurrencyFollowBaseRequestParameter : BaseRequestParameter
    {
        public UserCurrencyFollowBaseRequestParameter()
        {

        }
        public int[]? CurrencyId { get; set; }
        public DateTime? MinUpdatedDate { get; set; }
        public DateTime? MaxUpdatedDate { get; set; }
    }
}