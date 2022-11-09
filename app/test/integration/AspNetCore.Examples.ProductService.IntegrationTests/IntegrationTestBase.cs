using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RestEase;

namespace AspNetCore.Examples.ProductService
{
    public abstract class IntegrationTestBase
    {
        private CustomApplicationFactory? _applicationFactory;
        
        [SetUp]
        public void SetUp()
        {
            var factoryBuilder = new CustomApplicationFactoryBuilder();
            ConfigureApplicationFactory(factoryBuilder);
            _applicationFactory = factoryBuilder.Build();
        }

        protected virtual void ConfigureApplicationFactory(CustomApplicationFactoryBuilder builder)
        {
        }

        protected T GetService<T>() where T : notnull
        { 
            return _applicationFactory!.Services.GetRequiredService<T>();
        }

        protected TImplementation GetServiceImplementation<TService, TImplementation>() where TImplementation : notnull, TService
        {
            return (TImplementation) _applicationFactory!.Services.GetServices<TService>().First(x => x is TImplementation)!;
        }

        protected IProductServiceClient GetApplicationClient()
        {
            var httpClient = _applicationFactory!.CreateClient();
            return new RestClient(httpClient).For<IProductServiceClient>();
        }
    }
}