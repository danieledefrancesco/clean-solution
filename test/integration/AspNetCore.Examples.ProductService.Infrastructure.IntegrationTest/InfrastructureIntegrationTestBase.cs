using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Examples.ProductService
{
    public abstract class InfrastructureIntegrationTestBase
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected InfrastructureIntegrationTestBase()
        {
            var services = new ServiceCollection();
            var configuration = BuildConfiguration(new ConfigurationBuilder()).Build();
            ConfigureServices(services, configuration);
            ServiceProvider = services.BuildServiceProvider();
        }

        protected virtual IConfigurationBuilder BuildConfiguration(IConfigurationBuilder builder)
        {
            return builder.AddEnvironmentVariables();
        }

        protected virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDb(configuration);
        }
        
    }
}