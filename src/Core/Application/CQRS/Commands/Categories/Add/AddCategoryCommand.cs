using Application.CQRS.Commons.Interfaces;
using Domain;

namespace Application.CQRS.Commands.Categories.Add
{
    public sealed record AddCategoryCommand : ICommand
    {
        public AddCategoryCommand()
        {

        }
        public AddCategoryCommand(string title, bool isActive)
        {
            Title = title.TrimStart().TrimEnd();
            IsActive = isActive;
        }

        public string Title { get; init; } = null!;
        public bool IsActive { get; set; }

        internal Category ToDomain()
        {
            DateTime now = DateTime.UtcNow;
            return new Category
            {
                Title = this.Title,
                IsActive = this.IsActive,
                CreatedDate = now,
                UpdatedDate = now
            };
        }
    }
}