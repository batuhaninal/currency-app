using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Categories;

namespace Application.CQRS.Queries.Categories.List
{
    public sealed class CategoryListQuery : CategoryBaseRequestParameter, IQuery
    {
        public CategoryListQuery()
        {

        }
    }
}