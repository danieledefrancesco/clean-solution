using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCore.Examples.ProductService
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);
            var app = builder.Build();
            ApplicationConfigurator.ConfigureApp(app);
            await app.RunAsync();
        }

        public static WebApplicationBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ApplicationConfigurator.ConfigureBuilder(builder);
            return builder;
        }
        
        
    }
}


