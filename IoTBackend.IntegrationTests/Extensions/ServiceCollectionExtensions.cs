using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace IoTBackend.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void SwapTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
        {
            if (services.Any(x => x.ServiceType == typeof(TService) && x.Lifetime == ServiceLifetime.Transient))
            {
                var serviceDescriptors = services.Where(x => x.ServiceType == typeof(TService) && x.Lifetime == ServiceLifetime.Transient).ToList();
                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    services.Remove(serviceDescriptor);
                }
            }

            services.AddTransient(typeof(TService), (sp) => implementationFactory(sp));
        }
    }
}