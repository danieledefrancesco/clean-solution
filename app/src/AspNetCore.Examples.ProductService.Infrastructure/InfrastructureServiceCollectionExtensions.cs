using System;
using System.Net.Http;
using AspNetCore.Examples.ProductService.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Examples.ProductService
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkForSqlServer(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")!));
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());
            return services;
        }

        public static IServiceCollection AddDefaultHttpClientFactory(this IServiceCollection services) =>
            services.AddSingleton<IHttpClientFactory, DefaultHttpClientFactory>();
    }
}