using System.Reflection;
using AspNetCore.Examples.ProductService;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssembly(typeof(ProductValidator).Assembly);
    });
            
builder.Services.AddAutoMapper(Assembly.GetCallingAssembly());
            
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddEntityFrameworkForSqlServer()
    .AddDefaultHttpClientFactory()
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

app.MapControllers();

app.MapHealthChecks("/healthcheck");

app.Run();


void AddErrorHandlers(IServiceCollection services)
{
    services.AddScoped<IErrorHandlerFactory, ErrorHandlerFactory>();
    services.AddScoped<IErrorHandler, NotFoundErrorHandler>();
    services.AddScoped<IErrorHandler, AlreadyExistsErrorHandler>();
    services.AddScoped<IErrorHandler, PriceCardNewPriceLessThanZeroErrorHandler>();
    services.AddScoped<IErrorHandler, DefaultErrorHandler>();
}