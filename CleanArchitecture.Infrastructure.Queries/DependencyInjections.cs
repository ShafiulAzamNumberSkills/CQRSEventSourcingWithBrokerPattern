using CleanArchitecture.Application.Common.IRepositories.Queries;
using CleanArchitecture.Infrastructure.Common.Data;
using CleanArchitecture.Infrastructure.Queries.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CleanArchitecture.Infrastructure.Queries
{
    public static class DependencyInjections
    {

        public static IServiceCollection AddInfrastructureQueryServices(this IServiceCollection services, IConfiguration configuration)
        {


            var connectionString = configuration.GetConnectionString("DefaultConnection");


            services.AddDbContext<PostContext>(options =>
                 options.UseSqlServer(connectionString));

            services.AddScoped<IPostsRepository, PostsRepository>();


            return services;
        }


    }
}
