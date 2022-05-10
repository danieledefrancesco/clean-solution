using System.Net.Http;

namespace AspNetCore.Examples.ProductService.Factories
{
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}