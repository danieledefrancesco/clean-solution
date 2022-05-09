using AspNetCore.Examples.ProductService.ErrorHandlers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AspNetCore.Examples.ProductService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                });
            
            services.AddAutoMapper(typeof(Startup));
            
            services
                .AddApplicationServices(Configuration)
                .AddMongoDb(Configuration)
                .AddDefaultHttpClientFactory();
            
            AddErrorHandlers(services);
        }

        private static void AddErrorHandlers(IServiceCollection services)
        {
            services.AddScoped<IErrorHandlerFactory, ErrorHandlerFactory>();
            services.AddScoped<IErrorHandler, NotFoundErrorHandler>();
            services.AddScoped<IErrorHandler, AlreadyExistsErrorHandler>();
            services.AddScoped<IErrorHandler, PriceCardNewPriceLessThanZeroErrorHandler>();
            services.AddScoped<IErrorHandler, DefaultErrorHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}