using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Models.Currencies
{
    public sealed class CurrencyFilterViewModel
    {
        public CurrencyFilterViewModel()
        {

        }

        public CurrencyFilterViewModel(SelectList categories)
        {
            Categories = categories;
        }
        public bool IsActive { get; set; }
        public SelectList Categories { get; init; } = null!;
    }
}