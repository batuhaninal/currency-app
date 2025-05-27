using Application.CQRS.Commons.Interfaces;
using Application.Models.RequestParameters.Currencies;

namespace Application.CQRS.Queries.Currencies.List
{
    public sealed class CurrencyListQuery : CurrencyBaseRequestParameter, IQuery
    {
        public CurrencyListQuery()
        {

        }
    }
}