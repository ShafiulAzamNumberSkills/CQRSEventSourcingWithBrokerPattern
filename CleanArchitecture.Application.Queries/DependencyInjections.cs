using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CleanArchitecture.Application.Queries
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


            return services;
        }


    }
}
