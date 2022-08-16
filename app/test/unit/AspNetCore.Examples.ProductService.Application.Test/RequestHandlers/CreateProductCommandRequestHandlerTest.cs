using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    public class CreateProductCommandRequestHandlerTest
    {
        private CreateProductCommandRequestHandler _createProductCommandRequestHandler;
        private IProductRepository _productRepository;

        [SetUp]
        public void SetUp()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _createProductCommandRequestHandler = new CreateProductCommandRequestHandler(_productRepository);
        }

        [Test]
        public void CreateProduct_ReturnsAlreadyExists_IfProductAlreadyExists()
        {
            const string existingProductId = "p1";
            const string existingProductName = "Product 1";
            var existingProduct = new Product()
            {
                Id = existingProductId,
                Name = ProductName.From(existingProductName)
            };

            _productRepository.ExistsById(existingProductId).Returns(Task.FromResult(true));
            
            var response = _createProductCommandRequestHandler
                .Handle(new CreateProductCommandRequest(existingProduct), CancellationToken.None)
                .Result;
            
            response
                .IsT1
                .Should()
                .BeTrue();

            response
                .AsT1
                .Should()
                .BeOfType<AlreadyExistsError>();

        }
        
        [Test]
        public void CreateProduct_ReturnsCreateProductCommandResponse_IfProductDoesntExist()
        {
            const string productId = "p1";
            const string productName = "Product 1";
            var product = new Product()
            {
                Id = productId,
                Name = ProductName.From(productName)
            };

            var completedTask = Task.CompletedTask;
            _productRepository.ClearReceivedCalls();
            
            var response = _createProductCommandRequestHandler
                .Handle(new CreateProductCommandRequest(product), CancellationToken.None)
                .Result;
            
            response
                .IsT0
                .Should()
                .BeTrue();

            response
                .AsT0
                .CreatedProduct
                .Should()
                .Be(product);

            _productRepository
                .Received(1)
                .Insert(product);

        }
    }
}