using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNetQDI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            return UseRabbit(services, configuration.GetConnectionString("Rabbit"));
        }

        public static IServiceCollection UseRabbit(this IServiceCollection services, string connectionString)
        {
            services.RegisterEasyNetQ(connectionString);
            services.AddSingleton<MessageDispatcher>();

            return services;
        }

        public static IServiceCollection AddSubscriber<TService>(this IServiceCollection services)
            where TService : class
        {
            services.AddScoped<TService>();

            return services;
        }
    }
}
