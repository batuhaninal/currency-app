using Application.Abstractions.Repositories.Currencies;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Currencies
{
    public sealed class CurrencyTagWriteRepository : WriteRepository<CurrencyTag>, ICurrencyTagWriteRepository
    {
        public CurrencyTagWriteRepository(DbContext context) : base(context)
        {
        }
    }
}