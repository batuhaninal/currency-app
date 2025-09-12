using Application.Models.Constants.Messages;
using Application.Models.DTOs.Currencies;

namespace Application.Models.DTOs.UserCurrencyFollows
{
    public sealed record UserCurrencyFollowItemDto
    {
        public UserCurrencyFollowItemDto()
        {

        }

        public UserCurrencyFollowItemDto(int userCurrencyFollowId, int userId, int currencyId, DateTime updatedDate, bool isActive, CurrencyRelationDto? currency)
        {
            UserCurrencyFollowId = userCurrencyFollowId;
            UserId = userId;
            CurrencyId = currencyId;
            Currency = currency ?? new CurrencyRelationDto(0, ErrorMessage.PASSIVEorDELETED, ErrorMessage.PASSIVEorDELETED, 0, 0);
            UpdatedDate = updatedDate;
            IsActive = isActive;
        }

        public int UserCurrencyFollowId { get; init; }
        public int UserId { get; init; }
        public int CurrencyId { get; init; }
        public CurrencyRelationDto Currency { get; init; } = null!;
        public DateTime UpdatedDate { get; init; }
        public bool IsActive { get; init; }
    }
}