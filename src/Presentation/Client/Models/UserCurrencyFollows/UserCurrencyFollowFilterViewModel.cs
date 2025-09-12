using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Models.UserCurrencyFollows
{
    public sealed class UserCurrencyFollowFilterViewModel
    {
        public UserCurrencyFollowFilterViewModel()
        {
            
        }

        public UserCurrencyFollowFilterViewModel( SelectList currencies)
        {
            Currencies = currencies;
        }

        public bool IsActive { get; set; }
        public SelectList Currencies { get; init; } = null!;
    }    
}