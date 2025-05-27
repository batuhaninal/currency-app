using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Commons;

namespace Application.CQRS.Queries.Tools
{
    public sealed class GetCategoryToolListQuery : ToolRequestParameter, IQuery
    {
        public GetCategoryToolListQuery()
        {

        }
    }
}