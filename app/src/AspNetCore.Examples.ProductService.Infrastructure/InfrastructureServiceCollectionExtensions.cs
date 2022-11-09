using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspNetCore.Examples.ProductService.Behaviors;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Handlers;
using AspNetCore.Examples.ProductService.OutboxMessages;
using AspNetCore.Examples.ProductService.RemoteEventDefinitions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZapMicro.TransactionalOutbox.Configurations;

namespace AspNetCore.Examples.ProductService
{
    public static class InfrastructureServiceCollectionExtensions
    {
        private static readonly MethodInfo ConfigureTransactionalOutboxMessageHandler =
            typeof(ITransactionalOutboxConfigurationBuilder)
                .GetMethods()
                .First(method => method.Name == nameof(ITransactionalOutboxConfigurationBuilder.ConfigureOutboxMessageHandler) && method.GetParameters().Length == 0);

        private static List<IRemoteEventDefinition> _remoteEventDefinitions;

        private static List<IRemoteEventDefinition> RemoteEventDefinitions
        {
            get
            {
                if (_remoteEventDefinitions != null) return _remoteEventDefinitions;
                var remoteEventDefinitionType = typeof(IRemoteEventDefinition);
                _remoteEventDefinitions = typeof(InfrastructureServiceCollectionExtensions)
                    .Assembly
                    .GetExportedTypes()
                    .Where(type =>
                        type.IsClass && !type.IsAbstract && remoteEventDefinitionType.IsAssignableFrom(type))
                    .Select(type => (IRemoteEventDefinition)Activator.CreateInstance(type))
                    .ToList();

                return _remoteEventDefinitions;
            }
        }
        
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
            IConfiguration configuration) => services
                .AddEntityFrameworkForSqlServer()
                .AddPriceCardService()
                .AddTransactionalOutbox()
                .AddAzureStorageQueues(configuration);
        
        
        private static IServiceCollection AddEntityFrameworkForSqlServer(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")!));
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            return services;
        }


        private static IServiceCollection AddPriceCardService(this IServiceCollection services)
        {
            services.AddScoped<IPriceCardServiceClientFactory, PriceCardServiceClientFactory>()
                .AddScoped(sp => sp.GetRequiredService<IPriceCardServiceClientFactory>().Create())
                .AddHttpClient<IPriceCardServiceClientFactory, PriceCardServiceClientFactory>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            return services;
        }

        private static IServiceCollection AddTransactionalOutbox(this IServiceCollection services)
        {
            services.AddTransactionalOutbox<AppDbContext>(options =>
            {
                options.ConfigureDequeueOutboxMessagesConfiguration(new DequeueOutboxMessagesConfiguration());
                RemoteEventDefinitions.ForEach(
                    remoteEventDefinition => ConfigureTransactionalOutboxMessageHandler
                        .MakeGenericMethod(
                            typeof(EventOutboxMessageHandler<>).MakeGenericType(remoteEventDefinition.EventDtoType),
                            typeof(EventOutboxMessage<>).MakeGenericType(remoteEventDefinition.EventDtoType))
                        .Invoke(options, Array.Empty<object>()));
                return options;
            });
            
            RemoteEventDefinitions.ForEach(remoteEventDefinition =>
            {
                services.AddScoped(
                    typeof(INotificationHandler<>).MakeGenericType(remoteEventDefinition.DomainEventType),
                    typeof(TransactionalOutboxEventHandler<,>).MakeGenericType(remoteEventDefinition.DomainEventType, remoteEventDefinition.EventDtoType));
            });
            
            return services;
        }

        private static IServiceCollection AddAzureStorageQueues(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(new AzureStorageQueueClientFactoryConfiguration(configuration["QUEUE_STORAGE_CONNECTION_STRING"]));
            RemoteEventDefinitions.ForEach(remoteEventDefinition => services.AddAzureStorageQueueForEvent(remoteEventDefinition));
            return services;
        }

        private static IServiceCollection AddAzureStorageQueueForEvent(this IServiceCollection services, IRemoteEventDefinition remoteEventDefinition)
        {
            return services
                .AddSingleton(
                    typeof(IAzureStorageQueueClientFactory<>).MakeGenericType(remoteEventDefinition.EventDtoType),
                    typeof(AzureStorageQueueClientFactory<,>).MakeGenericType(remoteEventDefinition.DomainEventType,
                        remoteEventDefinition.EventDtoType))
                .AddScoped(
                    typeof(IQueueHandler<>).MakeGenericType(remoteEventDefinition.EventDtoType),
                    typeof(AzureStorageQueueHandler<>).MakeGenericType(remoteEventDefinition.EventDtoType));
        }
    }
}