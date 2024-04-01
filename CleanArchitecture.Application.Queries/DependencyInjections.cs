using CleanArchitecture.Infrastructure.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CleanArchitecture.Application
{ 
    public static class DependencyInjections
    {

        public static IServiceCollection AddApplicationQueryServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            //for mediatr only
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            //

            services.AddInfrastructureQueryServices(configuration);


            return services;
        }


    }
}
