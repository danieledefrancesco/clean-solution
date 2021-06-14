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
    public class GetProductByIdRequestHandlerTest
    {
        private GetProductByIdRequestHandler _getProductByIdRequestHandler;
        private IProductRepository _productRepository;

        [SetUp]
        public void SetUp()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _getProductByIdRequestHandler = new GetProductByIdRequestHandler(_productRepository);
        }

        [Test]
        public void Test_ReturnsNotFound_IfProductDoesntExist()
        {
            const string productId = "abc";
            var mockedResult = Task<Product>.FromResult((Product) null);
            _productRepository.GetById(productId).Returns(mockedResult);

            var request = new GetProductByIdRequest(productId);
            
            var response = _getProductByIdRequestHandler
                .Handle(request, default(CancellationToken))
                .Result;

            response
                .IsT1
                .Should()
                .BeTrue();

            response
                .AsT1
                .GetType()
                .Should()
                .Be<NotFoundError>();
            
        }
        
        [Test]
        public void Test_ReturnsProduct_IfProductExists()
        {
            const string productId = "abc";
            const string productName = "name";
            var mockedResult = Task<Product>.FromResult(new Product()
            {
                Id = productId,
                Name = ProductName.From(productName)
            });
            _productRepository.GetById(productId).Returns(mockedResult);

            var request = new GetProductByIdRequest(productId);
            
            var response = _getProductByIdRequestHandler
                .Handle(request, default(CancellationToken))
                .Result;

            response
                .IsT0
                .Should()
                .BeTrue();

            response
                .AsT0
                .Should()
                .NotBeNull();
            
            var product = response.AsT0.Product;

            product
                .Should()
                .NotBeNull();

            product
                .Id
                .Should()
                .Be(productId);

            product
                .Name
                .Value
                .Should()
                .Be(productName);

        }
    }
}