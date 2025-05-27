using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Commons;
using Application.Models.RequestParameters.Currencies;

namespace Application.CQRS.Queries.Tools.GetCurrencyToolList
{
    public sealed class GetCurrencyToolListQuery : ToolRequestParameter, IQuery
    {
        public GetCurrencyToolListQuery()
        {

        }
    }
}