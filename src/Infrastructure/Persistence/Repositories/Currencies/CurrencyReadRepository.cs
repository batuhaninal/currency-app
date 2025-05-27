using Application.Abstractions.Repositories.Currencies;
using Domain;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence
{
    public class CurrencyReadRepository : ReadRepository<Currency>, ICurrencyReadRepository
    {
        public CurrencyReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}