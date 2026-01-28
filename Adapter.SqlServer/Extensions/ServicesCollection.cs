using Adapter.SqlServer.Repository;
using Core.Models;
using Core.Ports;
using Core.Ports.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapter.SqlServer.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddSqlServerServices(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddContext(config)
                .AddInections();
            
            return services;
        }

        static IServiceCollection AddContext(this IServiceCollection services,IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DataBase");

            services.AddDbContext<TicketDbContext>(options =>
               options.UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly(typeof(TicketDbContext).Assembly.FullName)));

            // Identity configuration
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<TicketDbContext>()
                    .AddUserManager<UserManager<User>>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();

            return services;
        }

        static IServiceCollection AddInections(this IServiceCollection services)
        {
            services
                .AddScoped<IMatchRepo, MatchRepo>()
                .AddScoped<ITicketRepo, TicketRepo>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));

            return services;
        }
    }
}
