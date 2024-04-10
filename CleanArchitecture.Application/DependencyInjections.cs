using CleanArchitecture.Application.Commands.BrokerManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CleanArchitecture.Application.Commands
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

            services.AddScoped<IBrokerManager,RabbitMqManager>();

            return services;
        }


    }
}
