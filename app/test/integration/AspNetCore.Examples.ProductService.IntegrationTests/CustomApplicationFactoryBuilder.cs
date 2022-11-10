using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace AspNetCore.Examples.ProductService
{
    public sealed class CustomApplicationFactoryBuilder
    {
        private readonly IList<Action<IWebHostBuilder>> _configureBuilderActions = new List<Action<IWebHostBuilder>>();

        public T MockService<T>(params object[] args) where T : class
        {
            T mock = Substitute.For<T>(args);
            ReplaceService(mock);
            return mock;
        }

        public void ReplaceService<T>(T newImplementation) where T : class
        {
            _configureBuilderActions.Add(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.Remove(services.First(x => x.ServiceType == typeof(T)));
                    services.AddSingleton(newImplementation);
                });
            });
        }

        public void RemoveServices<T>() where T : class
        {
            _configureBuilderActions.Add(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    foreach (var service in services.Where(x => x.ServiceType == typeof(T)).ToList())
                    {
                        services.Remove(service);
                    }
                });
            });
        }

        public void RemoveHostedServices() => RemoveServices<IHostedService>();

        public CustomApplicationFactory Build()
        {
            void JointAction(IWebHostBuilder builder)
            {
                foreach (var action in _configureBuilderActions)
                {
                    action(builder);
                }
            }

            return new CustomApplicationFactory(JointAction);
        }
    }
}