using Application.CQRS.Commons.Interfaces;

namespace Application.CQRS.Queries.Categories.Info
{
    public sealed record CategoryInfoQuery : IQuery
    {
        public CategoryInfoQuery()
        {

        }
        public CategoryInfoQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
        public int CategoryId { get; init; }

    }
}