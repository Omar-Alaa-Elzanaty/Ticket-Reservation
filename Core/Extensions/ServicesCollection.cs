using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;
using System.Reflection;
namespace Core.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddApplicationservices(this IServiceCollection services)
        {
            services
                .AddMediator()
                .AddMapping()
                .AddValidators()
                .AddRedLock();

            return services;
        }

        static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ServicesCollection).Assembly);
            });

            return services;
        }

        static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        static IServiceCollection AddRedLock(this IServiceCollection services)
        {
            var endpoints = new List<RedLockEndPoint>
            {
                new DnsEndPoint("localhost", 6379)
            };

            var redLockFactory = RedLockFactory.Create(endpoints);

            services.AddSingleton<IDistributedLockFactory>(redLockFactory);
            return services;
        }
    }
}