using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.CreateProductCommand
{
    public sealed class CreateProductCommandRequestHandlerTest
    {
        private CreateProductCommandRequestHandler _createProductCommandRequestHandler;
        private IProductRepository _productRepository;
        private IProductsFactory _productsFactory;

        [SetUp]
        public void SetUp()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _productsFactory = Substitute.For<IProductsFactory>();
            _createProductCommandRequestHandler =
                new CreateProductCommandRequestHandler(_productRepository, _productsFactory);
        }

        [Test]
        public async Task CreateProduct_ReturnsAlreadyExists_IfProductAlreadyExists()
        {
            var existingProductId = ProductId.From("p1");
            var existingProductName = ProductName.From("Product 1");
            var existingProductPrice = ProductPrice.From(1);

            _productRepository.ExistsById(existingProductId).Returns(Task.FromResult(true));

            var response = await _createProductCommandRequestHandler
                .Handle(new CreateProductCommandRequest(existingProductId, existingProductName, existingProductPrice),
                    CancellationToken.None);

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
        public async Task CreateProduct_ReturnsCreateProductCommandResponse_IfProductDoesntExist()
        {
            var productId = ProductId.From("p1");
            var productName = ProductName.From("Product 1");
            var productPrice = ProductPrice.From(1);
            var product = new Product(productId, productName, productPrice);

            _productsFactory.CreateProduct(Arg.Any<ProductId>(), Arg.Any<ProductName>(), Arg.Any<ProductPrice>())
                .Returns(product);

            var response = await _createProductCommandRequestHandler
                .Handle(new CreateProductCommandRequest(productId, productName, productPrice), CancellationToken.None);

            response
                .IsT0
                .Should()
                .BeTrue();

            response
                .AsT0
                .CreatedProduct
                .Should()
                .Be(product);
        }
    }
}