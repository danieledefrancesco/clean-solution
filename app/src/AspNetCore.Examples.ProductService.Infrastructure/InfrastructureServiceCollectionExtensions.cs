using System;
using System.Net.Http;
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
        public static IServiceCollection AddEntityFrameworkForSqlServer(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")!));
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            return services;
        }

        public static IServiceCollection AddDefaultHttpClientFactory(this IServiceCollection services) =>
            services.AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>();

        public static IServiceCollection AddTransactionalOutbox(this IServiceCollection services) =>
            services.AddTransactionalOutbox<AppDbContext>(options => 
                options.ConfigureDequeueOutboxMessagesConfiguration(new DequeueOutboxMessagesConfiguration())
                    .ConfigureOutboxMessageHandler<EventOutboxMessageHandler<OnProductCreated>, EventOutboxMessage<OnProductCreated>>())
                .AddScoped<IEventHandler, TransactionalOutboxEventHandler>();

        public static IServiceCollection AddAzureStorageQueues(this IServiceCollection services, IConfiguration configuration) =>
            services.AddAzureStorageQueueForEvent<OnProductCreated>(configuration);

        private static IServiceCollection AddAzureStorageQueueForEvent<T>(this IServiceCollection services, IConfiguration configuration)
            where T : EventBase
        {
            Func<QueueClient> clientFactory = () => new QueueClient(configuration["QUEUE_STORAGE_CONNECTION_STRING"],
                nameof(T).ToLower());
            return services
                .AddSingleton(clientFactory)
                .AddScoped<IQueueHandler<T>>((sp) => new AzureStorageQueueHandler<T>(clientFactory()));
        }
    }
}