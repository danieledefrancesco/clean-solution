using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductByIdRequestHandlerTest
    {
        private GetProductByIdRequestHandler _getProductByIdRequestHandler;
        private IProductRepository _productRepository;

        [SetUp]
        public void SetUp()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _getProductByIdRequestHandler =
                new GetProductByIdRequestHandler(_productRepository);
        }

        [Test]
        public async Task Handle_ReturnsNotFound_IfProductDoesntExist()
        {
            var productId = ProductId.From("abc");
            var mockedResult = Task.FromResult((Product)null);
            _productRepository.GetById(productId).Returns(mockedResult);
            var request = new GetProductByIdRequest(productId);

            var response = await _getProductByIdRequestHandler
                .Handle(request, CancellationToken.None);

            response.IsT1.Should().BeTrue();

            response.AsT1.GetType().Should().Be<NotFoundError>();
        }

        [Test]
        public async Task Handle_ReturnsProduct_IfProductExists()
        {
            const string productId = "abc";
            const string productName = "name";
            const decimal productPrice = 10;

            var cancellationToken = default(CancellationToken);
            
            var request = new GetProductByIdRequest(ProductId.From(productId));

            var mockedResult = Task.FromResult(new Product(ProductId.From(productId), ProductName.From(productName),
                ProductPrice.From(productPrice)));
            
            _productRepository.GetById(ProductId.From(productId)).Returns(mockedResult);
            var response = await _getProductByIdRequestHandler
                .Handle(request, cancellationToken);

            response.IsT0.Should().BeTrue();
            response.AsT0.Should().NotBeNull();

            var product = response.AsT0.Product;
            product.Should().NotBeNull();
            product.Id.Value.Should().Be(productId);
            product.Name.Value.Should().Be(productName);
            product.Name.Value.Should().Be(productName);
            product.Price.Value.Should().Be(productPrice);
        }
    }
}