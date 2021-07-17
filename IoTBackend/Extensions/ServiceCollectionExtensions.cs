using System;
using System.IO;
using System.Reflection;
using Azure.Storage.Blobs;
using IoTBackend.Application.Handlers;
using IoTBackend.Application.Repositories;
using IoTBackend.Infrastructure.Repository;
using IoTBackend.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace IoTBackend.API.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void RegisterBlobStorage(this IServiceCollection services, string blobStorageConnectionString, string containerName)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(blobStorageConnectionString);
            });
            services.AddTransient<IStorageService>(sp => new AzureBlobStorageService(sp.GetService<BlobServiceClient>(), containerName));
        }
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
