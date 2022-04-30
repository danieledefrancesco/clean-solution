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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AspNetCore.Examples.ProductService.Test", Version = "v1"});
            });

            services
                .AddApplicationServices(Configuration)
                .AddMongoDb(Configuration);
            
            AddErrorHandlers(services);
        }

        private static void AddErrorHandlers(IServiceCollection services)
        {
            services.AddScoped<IErrorHandlerFactory, ErrorHandlerFactory>();
            services.AddScoped<IErrorHandler, NotFoundErrorHandler>();
            services.AddScoped<IErrorHandler, AlreadyExistsErrorHandler>();
            services.AddScoped<IErrorHandler, DefaultErrorHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCore.Examples.ProductService.Test v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}