using Client.Models.Currencies;
using Client.Models.Users;

namespace Client.Models.UserCurrencyFollows
{
    public sealed record UserCurrencyFollowInfoResponse
    {
        public UserCurrencyFollowInfoResponse()
        {

        }

        public UserCurrencyFollowInfoResponse(int userCurrencyFollowId, int userId, int currencyId, DateTime updatedDate, bool isActive, CurrencyRelationResponse currency, UserRelationResponse user)
        {
            UserCurrencyFollowId = userCurrencyFollowId;
            UserId = userId;
            CurrencyId = currencyId;
            Currency = currency;
            User = user;
            UpdatedDate = updatedDate;
            IsActive = isActive;
        }

        public int UserCurrencyFollowId { get; init; }
        public int UserId { get; init; }
        public UserRelationResponse User { get; init; } = null!;
        public int CurrencyId { get; init; }
        public CurrencyRelationResponse Currency { get; init; } = null!;
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
    }
    
    public sealed record UserCurrencyFollowItemResponse
    {
        public UserCurrencyFollowItemResponse()
        {

        }
        
        public UserCurrencyFollowItemResponse(int userCurrencyFollowId, int userId, int currencyId, DateTime updatedDate, bool isActive, CurrencyRelationResponse currency)
        {
            UserCurrencyFollowId = userCurrencyFollowId;
            UserId = userId;
            CurrencyId = currencyId;
            Currency = currency;
            UpdatedDate = updatedDate;
            IsActive = isActive;
        }

        public int UserCurrencyFollowId { get; init; }
        public int UserId { get; init; }
        public int CurrencyId { get; init; }
        public CurrencyRelationResponse Currency { get; init; } = null!;
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
    }
} 