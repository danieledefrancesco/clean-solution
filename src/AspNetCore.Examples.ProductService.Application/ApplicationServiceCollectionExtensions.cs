using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspNetCore.Examples.ProductService.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Examples.ProductService
{
    public static class ApplicationServiceCollectionExtensions
    {
        private static Assembly CurrentAssembly => typeof(ApplicationServiceCollectionExtensions).Assembly;
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddMediatR(CurrentAssembly)
                .AddRepositories(CurrentAssembly);
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var repositoryOpenGenericType = typeof(IRepository<,>);
            var repositoryInterfaces = assembly.GetExportedTypes().Where(@interface =>
                @interface.IsInterface &&
                IsNonGenericExtensionOfAGenericInterface(@interface, repositoryOpenGenericType));
            RegisterConcreteTypesForInterfaces(services, assembly, repositoryInterfaces);
            return services;
        }

        private static void RegisterConcreteTypesForInterfaces(IServiceCollection services, Assembly assembly, IEnumerable<Type> interfaces)
        {
            foreach (var @interface in interfaces)
            {
                RegisterConcreteTypeForInterface(services,assembly,@interface);
            }
        }

        private static void RegisterConcreteTypeForInterface(IServiceCollection services, Assembly assembly, Type @interface)
        {
            var concreteType = assembly.GetExportedTypes().FirstOrDefault(concreteType =>
                IsConcreteImplementationOfInterface(concreteType, @interface));
            if (concreteType == null)
            {
                return;
            }
            services.AddScoped(@interface, concreteType);
        }

        private static bool IsConcreteImplementationOfInterface(Type concreteType, Type @interface)
        {
            if (concreteType.IsInterface || concreteType.IsAbstract)
            {
                return false;
            }

            return @interface.IsAssignableFrom(concreteType);
        }

        public static bool IsNonGenericExtensionOfAGenericInterface(Type @interface, Type repositoryOpenGenericType)
        {
            foreach (var baseType in @interface.GetInterfaces())
            {
                if (!baseType.IsGenericType)
                {
                    continue;
                }
                
                var genericTypeDefinition = baseType.GetGenericTypeDefinition();

                if (repositoryOpenGenericType.IsAssignableFrom(genericTypeDefinition))
                {
                    return true;
                }
                
            }

            return false;
        }
    }
}