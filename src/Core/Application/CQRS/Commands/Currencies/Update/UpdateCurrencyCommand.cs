using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Commands.Currencies.Update
{
    public sealed record UpdateCurrencyCommand : ICommand
    {
        public UpdateCurrencyCommand()
        {

        }

        public UpdateCurrencyCommand(int currencyId, int categoryId, string title, string? subTitle, string? tvCode, bool isActive)
        {
            CurrencyId = currencyId;
            CategoryId = categoryId;
            Title = title;
            SubTitle = subTitle != null ? subTitle.TrimStart().TrimEnd() : null;
            TvCode = tvCode != null ? tvCode.TrimStart().TrimEnd() : null;
            IsActive = isActive;
        }

        [JsonIgnore]
        public int CurrencyId { get; set; }
        public int CategoryId { get; init; }
        public string Title { get; init; } = null!;
        public string? SubTitle { get; init; }
        public string? TvCode { get; init; }
        public bool IsActive { get; init; }
    }
}