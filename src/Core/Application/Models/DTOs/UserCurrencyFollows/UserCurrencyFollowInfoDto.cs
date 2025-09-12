using Application.Models.Constants.Messages;
using Application.Models.DTOs.Currencies;
using Application.Models.DTOs.Users;

namespace Application.Models.DTOs.UserCurrencyFollows
{
    public sealed record UserCurrencyFollowInfoDto
    {
        public UserCurrencyFollowInfoDto()
        {

        }

        public UserCurrencyFollowInfoDto(int userCurrencyFollowId, int userId, int currencyId, DateTime updatedDate, bool isActive, CurrencyRelationDto? currency, UserRelationDto? user)
        {
            UserCurrencyFollowId = userCurrencyFollowId;
            UserId = userId;
            CurrencyId = currencyId;
            Currency = currency ?? new CurrencyRelationDto(0, ErrorMessage.PASSIVEorDELETED, ErrorMessage.PASSIVEorDELETED, 0, 0);
            User = user ?? new UserRelationDto(0, ErrorMessage.PASSIVEorDELETED, ErrorMessage.PASSIVEorDELETED, ErrorMessage.PASSIVEorDELETED);
            UpdatedDate = updatedDate;
            IsActive = isActive;
        }

        public int UserCurrencyFollowId { get; init; }
        public int UserId { get; init; }
        public UserRelationDto User { get; init; } = null!;
        public int CurrencyId { get; init; }
        public CurrencyRelationDto Currency { get; init; } = null!;
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
    }
}