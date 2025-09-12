namespace Client.Models.UserCurrencyFollows
{
    public sealed record CurrencyFollowInput
    {
        public CurrencyFollowInput()
        {

        }

        public CurrencyFollowInput(int currencyId, bool isBroadcast)
        {
            CurrencyId = currencyId;
            IsBroadcast = isBroadcast;
        }

        public int CurrencyId { get; init; }
        public bool IsBroadcast { get; set; }
    }
    
    public sealed record ChangeCurrencyFollowInput
    {
        public ChangeCurrencyFollowInput()
        {
            
        }

        public ChangeCurrencyFollowInput(int userCurrencyFollowId, bool isActive)
        {
            UserCurrencyFollowId = userCurrencyFollowId;
            IsActive = isActive;
        }

        public int UserCurrencyFollowId { get; set; }
        public bool IsActive { get; init; }
    }
}