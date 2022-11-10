using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Handlers;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using AspNetCore.Examples.PriceCardService;
using NSubstitute;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public sealed class GetProductWithPriceCardByIdIntegrationTest : IntegrationTestBase
    {
        private readonly AppDbContext _appDbContext = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(nameof(AppDbContext))
                .Options);

        private PriceCardServiceClient? _priceCardServiceClient;

        protected override void ConfigureApplicationFactory(CustomApplicationFactoryBuilder builder)
        {
            _appDbContext.Products.RemoveRange(_appDbContext.Products.ToList());
            _appDbContext.SaveChanges();
            builder.ReplaceService(_appDbContext);
            builder.ReplaceService<DbContext>(_appDbContext);
            _priceCardServiceClient = builder.MockService<PriceCardServiceClient>(Substitute.For<HttpClient>());
            builder.MockService<IAzureStorageQueueClientFactory<OnProductCreatedEventDto>>();
            builder.MockService<IAzureStorageQueueClientFactory<OnProductUpdatedEventDto>>();
            builder.RemoveHostedServices();
        }

        private void ArrangePriceCardServiceClient(params PriceCard[] priceCards)
        {
            var priceCardList = new PriceCardList
            {
                Items = priceCards
            };
            var mockedResult = Task.FromResult(priceCardList);
            _priceCardServiceClient!.ActiveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(mockedResult);
        }

        [Test]
        public async Task GetProductWithPriceCardById_ReturnsProductWithNewPrice_IfProductAndActivePriceCardExist()
        {
            const string productId = "productId";
            const string productName = "productName";
            const decimal productPrice = 1;
            const string priceCardId = "priceCardId";
            const decimal priceCardNewPrice = 0.5m;
            var product = new Product(ProductId.From(productId), ProductName.From(productName), ProductPrice.From(productPrice));
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            ArrangePriceCardServiceClient(new PriceCard
            {
                Id = priceCardId,
                NewPrice = (double)priceCardNewPrice
            });
            var response = await GetApplicationClient().GetWithPriceCardById(productId);
            var productDto = response.GetContent();
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(productId);
            productDto.Name.Should().Be(productName);
            productDto.Price.Should().Be(productPrice);
            productDto.FinalPrice.Should().Be(priceCardNewPrice);
            productDto.PriceCard.Should().NotBeNull();
            productDto.PriceCard.Id.Should().Be(priceCardId);
            productDto.PriceCard.NewPrice.Should().Be(priceCardNewPrice);
        }
        
        [Test]
        public async Task GetProductWithPriceCardById_ReturnsProductWithoutNewPrice_IfActivePriceCardDoesntExist()
        {
            const string productId = "productId";
            const string productName = "productName";
            const decimal productPrice = 1;
            var product = new Product(ProductId.From(productId), ProductName.From(productName), ProductPrice.From(productPrice));
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            ArrangePriceCardServiceClient();
            var response = await GetApplicationClient().GetWithPriceCardById(productId);
            var productDto = response.GetContent();
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(productId);
            productDto.Name.Should().Be(productName);
            productDto.Price.Should().Be(productPrice);
            productDto.FinalPrice.Should().Be(productPrice);
            productDto.PriceCard.Should().BeNull();
        }
        
        [Test]
        public void GetProductWithPriceCardById_Returns404_IfProductDoesntExist()
        {
            GetApplicationClient()
                .Invoking(c => c.GetWithPriceCardById("productId").GetAwaiter().GetResult())
                .Should().Throw<RestEase.ApiException>()
                .Where(exc => exc.StatusCode == HttpStatusCode.NotFound);

        }
    }
}