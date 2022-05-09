using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AspNetCore.Examples.ProductService
{
    public static class Program
    {
        public static async Task Main(string[] args) =>
            await Task.Run(CreateHostBuilder(args).Build().Run);

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
    }
}
