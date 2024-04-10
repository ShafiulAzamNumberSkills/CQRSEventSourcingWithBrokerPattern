using CleanArchitecture.Application.Common.IRepositories.Commands;
using CleanArchitecture.Infrastructure.Commands.Repositories;
using CleanArchitecture.Infrastructure.Common.Data;
using CleanArchitecture.Infrastructure.Common.Data.EventStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CleanArchitecture.Infrastructure.Commands
{
    public static class DependencyInjections
    {

        public static IServiceCollection AddInfrastructureCommandServices(this IServiceCollection services, IConfiguration configuration)
        {


            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionStringEventDB = configuration.GetConnectionString("EventDBConnection");


            services.AddDbContext<PostContext>(options =>
                 options.UseSqlServer(connectionString));


            services.AddDbContext<EventContext>(options =>
                 options.UseSqlServer(connectionStringEventDB));

            services.AddScoped<IPostsRepository, PostsRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();


            return services;
        }


    }
}
