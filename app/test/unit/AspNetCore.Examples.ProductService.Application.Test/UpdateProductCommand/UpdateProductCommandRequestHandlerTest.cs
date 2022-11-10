using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.GetProductById;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using OneOf;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandRequestHandlerTest
    {
        private IMediator _mediator;
        private IProductRepository _productRepository;
        private UpdateProductCommandRequestHandler _updateProductCommandRequestHandler;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _productRepository = Substitute.For<IProductRepository>();
            _updateProductCommandRequestHandler = new UpdateProductCommandRequestHandler(_productRepository, _mediator);
        }
        
        [Test]
        public async Task Handle_ReturnsNotFound_IfProductDoesntExist()
        {
            var productId = ProductId.From("abc");
            var mockedResult = Task.FromResult((OneOf<GetProductByIdResponse, ErrorBase>)new NotFoundError());
            _mediator.Send(Arg.Any<GetProductByIdRequest>()).Returns(mockedResult);

            var request = new UpdateProductCommandRequest(productId, ProductName.From("name"), ProductPrice.From(1));

            var response = await _updateProductCommandRequestHandler
                .Handle(request, CancellationToken.None);

            response.IsT1.Should().BeTrue();

            response.AsT1.GetType().Should().Be<NotFoundError>();
        }
        
        [Test]
        public async Task Handle_UpdatesProduct_IfProductExists()
        {
            var productId = ProductId.From("abc");
            var oldProductName = ProductName.From("oldName");
            var newProductName = ProductName.From("newName");
            var oldProductPrice = ProductPrice.From(1);
            var newProductPrice = ProductPrice.From(2);
            
            var mockedResult = Task.FromResult((OneOf<GetProductByIdResponse, ErrorBase>)new GetProductByIdResponse(new Product(productId, oldProductName, oldProductPrice)));
            _mediator.Send(Arg.Any<GetProductByIdRequest>()).Returns(mockedResult);

            var task = Task.CompletedTask;
            _productRepository.Update(Arg.Any<Product>()).Returns(task);

            var request = new UpdateProductCommandRequest(productId, newProductName, newProductPrice);

            var response = await _updateProductCommandRequestHandler
                .Handle(request, CancellationToken.None);

            response.IsT0.Should().BeTrue();
            response.AsT0.UpdatedProduct.Should().NotBeNull();
            response.AsT0.UpdatedProduct.Id.Should().Be(productId);
            response.AsT0.UpdatedProduct.Name.Should().Be(newProductName);
            response.AsT0.UpdatedProduct.Price.Should().Be(newProductPrice);
            response.AsT0.UpdatedProduct.DomainEvents.Where(x => x is OnProductUpdated).Should().HaveCount(1);
            var domainEvent = (OnProductUpdated)response.AsT0.UpdatedProduct.DomainEvents.First(x => x is OnProductUpdated);
            domainEvent.ProductId.Should().Be(productId);
            domainEvent.ProductName.Should().Be(newProductName);
            domainEvent.ProductPrice.Should().Be(newProductPrice);
        }
    }
}