using Application.Abstractions.Repositories.Commons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void BindPersistenceServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContextPool<CurrencyContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PgSQL"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null
                        );
                    }
                ).EnableSensitiveDataLogging(environment.IsDevelopment()),
                poolSize: 128
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}