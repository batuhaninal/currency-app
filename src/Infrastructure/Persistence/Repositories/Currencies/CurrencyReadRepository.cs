using Application.Abstractions.Repositories.Currencies;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Currencies
{
    public class CurrencyReadRepository : ReadRepository<Currency>, ICurrencyReadRepository
    {
        public CurrencyReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}