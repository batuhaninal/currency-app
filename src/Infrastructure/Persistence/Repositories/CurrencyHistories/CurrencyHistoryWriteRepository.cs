using Application.Abstractions.Repositories.CurrencyHistories;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence
{
    public class CurrencyHistoryWriteRepository : WriteRepository<CurrencyHistory>, ICurrencyHistoryWriteRepository
    {
        public CurrencyHistoryWriteRepository(CurrencyContext context) : base(context)
        {
        }
    }
}