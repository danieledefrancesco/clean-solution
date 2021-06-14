using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace AspNetCore.Examples.ProductService
{
    internal static class LoggingConfiguration
    {
        internal static void ConfigureLogging(HostBuilderContext context, LoggerConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            configuration
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console();
            var elasticSearchUri = context.Configuration["ElasticConfiguration:Uri"];
            if (!string.IsNullOrWhiteSpace(elasticSearchUri) &&
                Uri.TryCreate(elasticSearchUri, UriKind.Absolute, out _))
            {
                configuration.WriteTo.Elasticsearch(ConfigureElasticSink(context.Configuration, environment));
                
            }
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                AutoRegisterTemplate = true,
                IndexFormat =
                    $"{configuration["APPLICATION_NAME"]}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}