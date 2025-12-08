using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts
{
    public class CurrencyContextFactory : IDesignTimeDbContextFactory<CurrencyContext>
    {
        public CurrencyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CurrencyContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=currency-app-db;User Id=Unravel;Password=Unr4vel!;");

            return new CurrencyContext(optionsBuilder.Options);
        }
    }
}