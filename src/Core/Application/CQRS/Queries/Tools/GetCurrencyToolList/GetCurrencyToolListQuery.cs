using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Currencies;

namespace Application.CQRS.Queries.Tools.GetCurrencyToolList
{
    public sealed class GetCurrencyToolListQuery : CurrencyBaseRequestParameter, IQuery
    {
        public GetCurrencyToolListQuery()
        {

        }
    }
}