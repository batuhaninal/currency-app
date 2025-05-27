using Application.CQRS.Commons.Interfaces;
using Domain;

namespace Application.CQRS.Commands.Currencies.Add
{
    public sealed record AddCurrencyCommand : ICommand
    {
        public AddCurrencyCommand()
        {

        }

        public AddCurrencyCommand(string title, string? subTitle, string? tvCode, decimal purchasePrice, decimal salePrice, int categoryId, bool isActive)
        {
            Title = title.TrimStart().TrimEnd();
            SubTitle = subTitle != null ? subTitle.TrimStart().TrimEnd() : null;
            TVCode = tvCode != null ? tvCode.TrimStart().TrimEnd() : null;
            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            CategoryId = categoryId;
            IsActive = isActive;
        }

        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TVCode { get; init; }
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public int CategoryId { get; init; }
        public bool IsActive { get; init; }

        internal Currency ToDomain()
        {
            DateTime now = DateTime.UtcNow;

            return new Currency
            {
                Title = this.Title,
                SubTitle = this.SubTitle,
                TVCode = this.TVCode,
                PurchasePrice = this.PurchasePrice,
                SalePrice = this.SalePrice,
                CategoryId = this.CategoryId,
                IsActive = this.IsActive,
                CreatedDate = now,
                UpdatedDate = now,
            };
        }
    }
}