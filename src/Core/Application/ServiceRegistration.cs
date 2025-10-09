using Application.CQRS.Commons.Behaviours;
using Application.CQRS.Commands;
using Application.CQRS.Commons;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Application.CQRS.Commands.Categories.Add;

namespace Application 
{
    public static class ServiceRegistration
    {
        public static void BindApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Dispatcher>();
            // Pipeline sirasi onemli! Cunku dispatcherda DI uzerinde servis basvurularinin listesini alip tersine calistirarak calistiriyoruz.
            services.AddScoped(typeof(IPipeline<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipeline<,>), typeof(PerformanceBehaviour<,>));
            
            services.AddValidatorsFromAssembly(typeof(AddCategoryCommand).Assembly);
            services.BindQueries();
            services.BindCommands();
        }
    }
}