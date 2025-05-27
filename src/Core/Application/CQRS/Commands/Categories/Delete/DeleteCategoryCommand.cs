using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Commands.Categories.Delete
{
    public sealed record DeleteCategoryCommand : ICommand
    {
        public DeleteCategoryCommand()
        {

        }

        public DeleteCategoryCommand(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; init; }
    }
}