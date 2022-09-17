using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.Examples.ProductService.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Examples.ProductService
{
    public static class DependencyInjectionExtensions
    {
        public static void MapEndpoints(this IEndpointRouteBuilder webApp)
        {
            GetConcreteEndpointTypes()
                .Select(CreateEndpointInstance)
                .ToList()
                .ForEach(webApp.MapEndpoint);
        }

        internal static IEndpoint CreateEndpointInstance(Type type)
        {
            return (IEndpoint)Activator.CreateInstance(type);
        }

        internal static IEnumerable<Type> GetConcreteEndpointTypes()
        {
            return typeof(DependencyInjectionExtensions).Assembly
                .GetExportedTypes()
                .Where(x => x.IsAssignableTo(typeof(IEndpoint)))
                .Where(x => x.IsClass && !x.IsAbstract);
        }

        internal static void MapEndpoint(this IEndpointRouteBuilder webApp, IEndpoint endpoint)
        {
            webApp.MapMethods(endpoint.Patten, endpoint.Methods, endpoint.Delegate);
        }
    }
}