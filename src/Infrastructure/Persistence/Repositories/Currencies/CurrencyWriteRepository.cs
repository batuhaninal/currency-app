using Application.Abstractions.Repositories.Currencies;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Currencies
{
    public class CurrencyWriteRepository : WriteRepository<Currency>, ICurrencyWriteRepository
    {
        public CurrencyWriteRepository(CurrencyContext context) : base(context)
        {
        }
    }
}