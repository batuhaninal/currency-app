using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Models.Assets
{
    public sealed record AssetFilterViewModel
    {
        public AssetFilterViewModel()
        {

        }

        public AssetFilterViewModel(SelectList currencies)
        {
            Currencies = currencies;
        }

        public SelectList Currencies { get; init; } = null!;
    }
}