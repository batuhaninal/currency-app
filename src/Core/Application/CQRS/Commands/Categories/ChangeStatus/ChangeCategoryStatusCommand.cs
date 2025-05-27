using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Commands.Categories.ChangeStatus
{
    public sealed record ChangeCategoryStatusCommand : ICommand
    {
        public ChangeCategoryStatusCommand()
        {

        }

        public ChangeCategoryStatusCommand(int categoryId)
        {
            CategoryId = categoryId;
        }
        public int CategoryId { get; init; }
    }
}