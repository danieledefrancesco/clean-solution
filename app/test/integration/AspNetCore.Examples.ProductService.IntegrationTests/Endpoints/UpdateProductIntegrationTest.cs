using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Handlers;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using RestEase;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public sealed class UpdateProductByIdIntegrationTest : IntegrationTestBase
    {
        private readonly AppDbContext _appDbContext = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(nameof(AppDbContext))
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options);

        protected override void ConfigureApplicationFactory(CustomApplicationFactoryBuilder builder)
        {
            _appDbContext.Products.RemoveRange(_appDbContext.Products.ToList());
            _appDbContext.SaveChanges();
            builder.ReplaceService(_appDbContext);
            builder.ReplaceService<DbContext>(_appDbContext);
            builder.MockService<IAzureStorageQueueClientFactory<OnProductCreatedEventDto>>();
            builder.MockService<IAzureStorageQueueClientFactory<OnProductUpdatedEventDto>>();
            builder.RemoveHostedServices();
        }

        [Test]
        public async Task UpdateProduct_ReturnsProduct_IfProductExists()
        {
            const string productId = "productId";
            const string productName = "productName";
            const decimal productPrice = 1;
            const string newName = "newName";
            const decimal newPrice = 2;
            var product = new Product(ProductId.From(productId), ProductName.From(productName),
                ProductPrice.From(productPrice));
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            var response = await GetApplicationClient().Update(productId, new UpdateProductCommandRequestDtoBody
            {
                Name = newName,
                Price = newPrice
            });
            var productDto = response.GetContent();
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(productId);
            productDto.Name.Should().Be(newName);
            productDto.Price.Should().Be(newPrice);
            product = await _appDbContext.Products.FirstAsync(p => p.Id == ProductId.From(productId));
            product.Name.Value.Should().Be(newName);
            product.Price.Value.Should().Be(newPrice);
        }

        [Test]
        public void UpdateProduct_Returns404_IfProductDoesntExist()
        {
            GetApplicationClient()
                .Invoking(c => c.Update("productId", new UpdateProductCommandRequestDtoBody
                {
                    Name = "name",
                    Price = 1
                }).GetAwaiter().GetResult())
                .Should().Throw<ApiException>()
                .Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }
    }
}