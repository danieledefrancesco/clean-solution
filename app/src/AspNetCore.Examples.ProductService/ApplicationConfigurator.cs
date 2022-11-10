using System;
using System.Linq;
using System.Reflection;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCore.Examples.ProductService
{
    public static class ApplicationConfigurator
    {
        private static readonly Assembly assembly = typeof(ApplicationConfigurator).Assembly; 
        public static void ConfigureBuilder(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services
                .AddControllers(AddModelBinders)
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(typeof(ProductValidator).Assembly);
                });
            
            builder.Services.AddAutoMapper(assembly);
            
            builder.Services
                .AddApplicationLayer(builder.Configuration)
                .AddInfrastructureLayer(builder.Configuration);
            
            AddErrorHandlers(builder.Services);
            builder.Services.AddHealthChecks();
        }

        public static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/healthcheck");
        }
        
        private static void AddErrorHandlers(IServiceCollection services)
        {
            services.AddScoped<IErrorHandlerFactory, ErrorHandlerFactory>();
            services.AddScoped<IErrorHandler, NotFoundErrorHandler>();
            services.AddScoped<IErrorHandler, AlreadyExistsErrorHandler>();
            services.AddScoped<IErrorHandler, PriceCardNewPriceLessThanZeroErrorHandler>();
            services.AddScoped<IErrorHandler, DefaultErrorHandler>();
        }

        private static void AddModelBinders(MvcOptions options)
        {
            var modelBinderProviderType = typeof(IModelBinderProvider);
            assembly
                .ExportedTypes
                .Where(x => modelBinderProviderType.IsAssignableFrom(x))
                .Select(x => (IModelBinderProvider)Activator.CreateInstance(x))
                .ToList()
                .ForEach(x => options.ModelBinderProviders.Insert(0, x));
        }
    }
}