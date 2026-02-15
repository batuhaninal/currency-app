using Application.Abstractions.Repositories.Currencies;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Currencies
{
    public sealed class CurrencyTagReadRepository : ReadRepository<CurrencyTag>, ICurrencyTagReadRepository
    {
        public CurrencyTagReadRepository(DbContext context) : base(context)
        {
        }
    }
}