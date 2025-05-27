using Application.Abstractions.Repositories.CurrencyHistories;
using Domain;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence
{
    public class CurrencyHistoryReadRepository : ReadRepository<CurrencyHistory>, ICurrencyHistoryReadRepository
    {
        public CurrencyHistoryReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}