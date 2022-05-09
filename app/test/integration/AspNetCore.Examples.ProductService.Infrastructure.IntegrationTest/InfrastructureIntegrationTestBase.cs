using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Examples.ProductService
{
    public abstract class InfrastructureIntegrationTestBase
    {
        protected IServiceProvider ServiceProvider { get; }

        protected InfrastructureIntegrationTestBase()
        {
            var services = new ServiceCollection();
            var configuration = BuildConfiguration(new ConfigurationBuilder()).Build();
            ConfigureServices(services, configuration);
            ServiceProvider = services.BuildServiceProvider();
        }

        private IConfigurationBuilder BuildConfiguration(IConfigurationBuilder builder)
        {
            return builder.AddEnvironmentVariables();
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDb(configuration);
        }
        
    }
}