using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace AspNetCore.Examples.ProductService
{
    public sealed class CustomApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly Action<IWebHostBuilder> _configureBuilder;

        public CustomApplicationFactory(Action<IWebHostBuilder> configureBuilder)
        {
            _configureBuilder = configureBuilder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _configureBuilder(builder);
        }
    }
}