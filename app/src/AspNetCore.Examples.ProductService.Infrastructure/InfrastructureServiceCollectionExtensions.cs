using System;
using AspNetCore.Examples.ProductService.Behaviors;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Handlers;
using AspNetCore.Examples.ProductService.OutboxMessages;
using Azure.Storage.Queues;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZapMicro.TransactionalOutbox.Configurations;

namespace AspNetCore.Examples.ProductService
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
            IConfiguration configuration) => services
                .AddEntityFrameworkSqlServer()
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

        private static IServiceCollection AddTransactionalOutbox(this IServiceCollection services) =>
            services.AddTransactionalOutbox<AppDbContext>(options => 
                options.ConfigureDequeueOutboxMessagesConfiguration(new DequeueOutboxMessagesConfiguration())
                    .ConfigureOutboxMessageHandler<EventOutboxMessageHandler<OnProductCreatedEventDto>, EventOutboxMessage<OnProductCreatedEventDto>>())
                .AddScoped<INotificationHandler<OnProductCreated>, TransactionalOutboxEventHandler<OnProductCreated, OnProductCreatedEventDto>>();

        private static IServiceCollection AddAzureStorageQueues(this IServiceCollection services, IConfiguration configuration) =>
            services.AddAzureStorageQueueForEvent<OnProductCreated, OnProductCreatedEventDto>(configuration);

        private static IServiceCollection AddAzureStorageQueueForEvent<TEvent, TDto>(this IServiceCollection services, IConfiguration configuration)
        {
            QueueClient ClientFactory() => new (configuration["QUEUE_STORAGE_CONNECTION_STRING"], typeof(TEvent)!.Name!.ToLower());
            return services
                .AddSingleton((Func<QueueClient>)ClientFactory)
                .AddScoped<IQueueHandler<TDto>>((_) => new AzureStorageQueueHandler<TDto>(ClientFactory()));
        }
    }
}