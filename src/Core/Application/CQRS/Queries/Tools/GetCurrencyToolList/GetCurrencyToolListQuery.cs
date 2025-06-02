using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Commons;

namespace Application.CQRS.Queries.Tools.GetCurrencyToolList
{
    public sealed class GetCurrencyToolListQuery : ToolRequestParameter, IQuery
    {
        public GetCurrencyToolListQuery()
        {

        }
    }
}