using System.Net.Http;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Persistence;
using AspNetCore.Examples.ProductService.Persistence.ClassMapRegistrationProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Examples.ProductService
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMongoDb(configuration, new MongoDbClassMapsRegister());
        }
        
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration, IMongoDbClassMapsRegister  classMapsRegister)
        {
            classMapsRegister.RegisterAllClassMaps();
            return services
                .Configure<MongoDbConfiguration>(options => configuration.GetSection(nameof(MongoDbConfiguration)).Bind(options))
                .AddScoped<IMongoDbProvider,MongoDbProvider>()
                .AddScoped(typeof(IPersistenceImplementation<,>), typeof(MongoDbPersistenceImplementation<,>));

        }

        public static IServiceCollection AddDefaultHttpClientFactory(this IServiceCollection services) =>
            services.AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>();
    }

}