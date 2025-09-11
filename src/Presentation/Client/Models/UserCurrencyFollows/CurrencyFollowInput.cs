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
}