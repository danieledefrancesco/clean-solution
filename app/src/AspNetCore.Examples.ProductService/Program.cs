using System.Reflection;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService;
using AspNetCore.Examples.ProductService.Endpoints;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.RequestHandlers;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddFluentValidationRulesScoped();  
});
builder.Services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
            
builder.Services.AddAutoMapper(Assembly.GetCallingAssembly());
            
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddEntityFrameworkForSqlServer()
    .AddPriceCardService()
    .AddTransactionalOutbox()
    .AddAzureStorageQueues(builder.Configuration);
            
AddErrorHandlers(builder.Services);
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/healthcheck");

MapEndpoints(app);

app.Run();


void AddErrorHandlers(IServiceCollection services)
{
    services.AddScoped<IErrorHandlerFactory, ErrorHandlerFactory>();
    services.AddScoped<IErrorHandler, NotFoundErrorHandler>();
    services.AddScoped<IErrorHandler, AlreadyExistsErrorHandler>();
    services.AddScoped<IErrorHandler, PriceCardNewPriceLessThanZeroErrorHandler>();
    services.AddScoped<IErrorHandler, DefaultErrorHandler>();
}

void MapEndpoints(WebApplication webApp)
{
    MapEndpoint(webApp, new GetProductEndpoint());
    MapEndpoint(webApp, new CreateProductEndpoint());
}

void MapEndpoint(WebApplication webApp, IEndpoint endpoint)
{
    webApp.MapMethods(endpoint.Patten, endpoint.Methods, endpoint.Delegate);
}

