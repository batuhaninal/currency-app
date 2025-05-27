using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Categories;

namespace Application.CQRS.Queries.Tools
{
    public sealed class GetCategoryToolListQuery : CategoryBaseRequestParameter, IQuery
    {
        public GetCategoryToolListQuery()
        {

        }

        protected override int MaxSize { get => base.MaxSize; set => base.MaxSize = 1; }
        protected override int DefaultSize { get => base.DefaultSize; set => base.DefaultSize = 1; }
    }
}