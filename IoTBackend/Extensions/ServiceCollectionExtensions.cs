using System;
using System.IO;
using System.Reflection;
using IoTBackend.Application.Handlers;
using IoTBackend.Application.Repositories;
using IoTBackend.Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IoTBackend.API.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IDeviceMeasurementRepository, DeviceMeasurementRepository>();
        }

        internal static void RegisterMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GetDeviceMeasurementsQueryHandler).Assembly);
        }

        internal static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }
    }
}
