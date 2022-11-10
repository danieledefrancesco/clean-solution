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
    public sealed class CreateProductIntegrationTest : IntegrationTestBase
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
        public async Task CreateProduct_ReturnCreatedProduct_IfProductExists()
        {
            const string productId = "productId";
            const string productName = "productName";
            const decimal productPrice = 1;
            var response = await GetApplicationClient().CreatePost(new CreateProductCommandRequestDto
            {
                Id = productId,
                Name = productName,
                Price = productPrice
            });
            var product = response.GetContent();
            product.Id.Should().Be(productId);
            product.Name.Should().Be(productName);
            product.Price.Should().Be(productPrice);
        }

        [Test]
        public async Task CreateProduct_Returns409ConflictError_IfProductAlreadyExists()
        {
            const string productId = "productId";
            const string productName = "productName";
            const decimal productPrice = 1;
            var product = new Product(
                ProductId.From(productId),
                ProductName.From(productName),
                ProductPrice.From(productPrice));
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            GetApplicationClient()
                .Invoking(client => client.CreatePost(new CreateProductCommandRequestDto
                {
                    Id = productId,
                    Name = productName,
                    Price = productPrice
                }).GetAwaiter().GetResult())
                .Should()
                .Throw<ApiException>()
                .Where(exc => exc.StatusCode == HttpStatusCode.Conflict);
        }
    }
}