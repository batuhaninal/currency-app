using Application.Abstractions.Handlers;
using Application.CQRS.Commands;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application 
{
    public static class ServiceRegistration
    {
        public static void BindApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Dispatcher>();
            services.BindQueries();
            services.BindCommands();
        }
    }
}