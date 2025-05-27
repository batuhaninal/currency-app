using Application.Abstractions.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void BindPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CurrencyContext>(options => options.UseNpgsql(configuration.GetConnectionString("PgSQL")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}