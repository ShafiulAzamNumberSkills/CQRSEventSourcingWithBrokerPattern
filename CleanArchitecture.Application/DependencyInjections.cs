using CleanArchitecture.Application.Commands.BrokerManager;
using CleanArchitecture.Infrastructure.Commands;
using CleanArchitecture.Infrastructure.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;


namespace CleanArchitecture.Application
{ 
    public static class DependencyInjections
    {

        public static IServiceCollection AddApplicationCommandServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            //for mediatr only
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            //

            services.AddInfrastructureCommandServices(configuration);
            services.AddInfrastructureQueryServices(configuration);

            services.AddScoped<IBrokerManager,RabbitMqManager>();

            return services;
        }


    }
}
