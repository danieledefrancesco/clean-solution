using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    public class GetProductByIdRequestHandlerTest
    {
        private GetProductByIdRequestHandler _getProductByIdRequestHandler;
        private IProductRepository _productRepository;
        private IPriceCardServiceClientFactory _priceCardServiceClientFactory;
        private PriceCardServiceClient _priceCardClient;

        [SetUp]
        public void SetUp()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _priceCardServiceClientFactory = Substitute.For<IPriceCardServiceClientFactory>();
            _priceCardClient = Substitute.For<PriceCardServiceClient>(new object[] { Substitute.For<HttpClient>()});
            _priceCardServiceClientFactory.Create().Returns(_priceCardClient);
            _getProductByIdRequestHandler = new GetProductByIdRequestHandler(_productRepository, _priceCardServiceClientFactory);
        }

        [Test]
        public void Handle_ReturnsNotFound_IfProductDoesntExist()
        {
            const string productId = "abc";
            var mockedResult = Task.FromResult((Product) null);
            _productRepository.GetById(productId).Returns(mockedResult);

            var request = new GetProductByIdRequest(productId);
            
            var response = _getProductByIdRequestHandler
                .Handle(request, default(CancellationToken))
                .Result;

            response.IsT1.Should().BeTrue();

            response.AsT1.GetType().Should().Be<NotFoundError>();
            
        }
        
        [Test]
        public void Handle_ReturnsPriceCardNewPriceIsLowerThanZero_IfProductExists()
        {
            const string productId = "abc";
            const string productName = "name";
            const decimal productPrice = 10;
            
            var cancellationToken = default(CancellationToken);

            var mockedPriceCardList = Task.FromResult(new PriceCardList()
            {
                Items = new List<PriceCard>
                {
                    new()
                    {
                        ProductId = productId,
                        NewPrice = -1,
                        Id = "pc1"
                    }
                }
            });
            
            var mockedResult = Task.FromResult(new Product()
            {
                Id = productId,
                Name = ProductName.From(productName),
                Price = ProductPrice.From(productPrice)
            });
            
            _productRepository.GetById(productId).Returns(mockedResult);
            _priceCardClient.ActiveAsync(productId, cancellationToken).Returns(mockedPriceCardList);

            var request = new GetProductByIdRequest(productId);
            
            var response = _getProductByIdRequestHandler
                .Handle(request, cancellationToken)
                .Result;

            response.IsT1.Should().BeTrue();
            response.AsT1.Should().NotBeNull();
            response.AsT1.Should().BeOfType<PriceCardNewPriceLessThanZeroError>();
        }
        
        [Test]
        public void Handle_ReturnsProduct_IfProductExists()
        {
            const string productId = "abc";
            const string productName = "name";
            const decimal productPrice = 10;
            
            var cancellationToken = default(CancellationToken);

            var mockedPriceCardList = Task.FromResult(new PriceCardList()
            {
                Items = new List<PriceCard>()
            });
            
            var mockedResult = Task.FromResult(new Product()
            {
                Id = productId,
                Name = ProductName.From(productName),
                Price = ProductPrice.From(productPrice)
            });
            
            _productRepository.GetById(productId).Returns(mockedResult);
            _priceCardClient.ActiveAsync(productId, cancellationToken).Returns(mockedPriceCardList);

            var request = new GetProductByIdRequest(productId);
            
            var response = _getProductByIdRequestHandler
                .Handle(request, cancellationToken)
                .Result;

            response.IsT0.Should().BeTrue();
            response.AsT0.Should().NotBeNull();
            
            var product = response.AsT0.Product;
            product.Should().NotBeNull();
            product.Id.Should().Be(productId);
            product.Name.Value.Should().Be(productName);
            product.Name.Value.Should().Be(productName);
            product.Price.Value.Should().Be(productPrice);
            product.FinalPrice.Value.Should().Be(productPrice);
        }
        
        [Test]
        public void Handle_ReturnsProductWithPriceCardPrice_IfProductAndPriceCardExist()
        {
            const string productId = "abc";
            const string productName = "name";
            const decimal productPrice = 10;
            const double priceCardPrice = 8;
            
            var cancellationToken = default(CancellationToken);

            var mockedPriceCardList = Task.FromResult(new PriceCardList()
            {
                Items = new List<PriceCard>()
                {
                    new()
                    {
                        NewPrice = priceCardPrice
                    }
                }
            });
            
            var mockedResult = Task.FromResult(new Product()
            {
                Id = productId,
                Name = ProductName.From(productName),
                Price = ProductPrice.From(productPrice)
            });
            
            _productRepository.GetById(productId).Returns(mockedResult);
            _priceCardClient.ActiveAsync(productId, cancellationToken).Returns(mockedPriceCardList);

            var request = new GetProductByIdRequest(productId);
            
            var response = _getProductByIdRequestHandler
                .Handle(request, cancellationToken)
                .Result;

            response.IsT0.Should().BeTrue();
            response.AsT0.Should().NotBeNull();
            
            var product = response.AsT0.Product;
            product.Should().NotBeNull();
            product.Id.Should().Be(productId);
            product.Name.Value.Should().Be(productName);
            product.Price.Value.Should().Be(System.Convert.ToDecimal(productPrice));
            product.FinalPrice.Value.Should().Be(System.Convert.ToDecimal(priceCardPrice));
        }
    }
}