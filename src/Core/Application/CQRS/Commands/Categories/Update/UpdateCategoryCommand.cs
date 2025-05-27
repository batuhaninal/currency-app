using System.Text.Json.Serialization;
using Application.CQRS.Commons.Interfaces;
using Domain;

namespace Application.CQRS.Commands.Categories.Update
{
    public sealed record UpdateCategoryCommand : ICommand
    {
        public UpdateCategoryCommand()
        {

        }

        public UpdateCategoryCommand(int categoryId, string title, bool isActive)
        {
            CategoryId = categoryId;
            Title = title;
            IsActive = isActive;
        }

        [JsonIgnore]
        public int CategoryId { get; set; }
        public string Title { get; init; } = null!;
        public bool IsActive { get; init; }

        internal void ToUpdate(ref Category category)
        {
            category.Title = Title;
            category.IsActive = IsActive;
            category.UpdatedDate = DateTime.UtcNow;
        }
    }
}