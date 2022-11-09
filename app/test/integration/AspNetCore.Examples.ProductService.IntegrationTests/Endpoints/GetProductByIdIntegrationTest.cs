using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Handlers;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestEase;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public sealed class GetProductByIdIntegrationTest : IntegrationTestBase
    {
        private readonly AppDbContext _appDbContext = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(nameof(AppDbContext))
                .Options);

        protected override void ConfigureApplicationFactory(CustomApplicationFactoryBuilder builder)
        {
            _appDbContext.Products.RemoveRange(_appDbContext.Products.ToList());
            builder.ReplaceService(_appDbContext);
            builder.ReplaceService<DbContext>(_appDbContext);
            builder.MockService<IAzureStorageQueueClientFactory<OnProductCreatedEventDto>>();
            builder.MockService<IAzureStorageQueueClientFactory<OnProductUpdatedEventDto>>();
            builder.RemoveHostedServices();
        }

        [Test]
        public async Task GetProductById_ReturnsProduct_IfProductExists()
        {
            const string productId = "productId";
            const string productName = "productName";
            const decimal productPrice = 1;
            var product = new Product(ProductId.From(productId), ProductName.From(productName), ProductPrice.From(productPrice));
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            var response = await GetApplicationClient().GetById(productId);
            var productDto = response.GetContent();
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(productId);
            productDto.Name.Should().Be(productName);
            productDto.Price.Should().Be(productPrice);
        }
        
        [Test]
        public void GetProductById_Returns404_IfProductDoesntExist()
        {
            GetApplicationClient()
                .Invoking(c => c.GetById("productId").GetAwaiter().GetResult())
                .Should().Throw<ApiException>()
                .Where(exc => exc.StatusCode == HttpStatusCode.NotFound);

        }
    }
}