using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.GetProductById;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using OneOf;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdRequestHandlerTest
    {
        private GetProductWithPriceCardByIdRequestHandler _getProductWithPriceCardByIdRequestHandler;
        private IMediator _mediator;
        private PriceCardServiceClient _priceCardServiceClient;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _priceCardServiceClient =
                Substitute.For<PriceCardServiceClient>(new object[]{ Substitute.For<HttpClient>() });
            _getProductWithPriceCardByIdRequestHandler =
                new GetProductWithPriceCardByIdRequestHandler(_mediator, _priceCardServiceClient);
        }

        [Test]
        public async Task Handle_ReturnsNotFound_IfProductDoesntExist()
        {
            var productId = ProductId.From("abc");
            var mockedResult = Task.FromResult((OneOf<GetProductByIdResponse, IError>)new NotFoundError());
            _mediator.Send(Arg.Any<GetProductByIdRequest>()).Returns(mockedResult);

            var request = new GetProductWithPriceCardByIdRequest(productId);

            var response = await _getProductWithPriceCardByIdRequestHandler
                .Handle(request, CancellationToken.None);

            response.IsT1.Should().BeTrue();

            response.AsT1.GetType().Should().Be<NotFoundError>();
        }

        [Test]
        public async Task Handle_ReturnsPriceCardNewPriceIsLowerThanZero_IfProductExistsAndPriceCardNewPriceIsLowerThanZero()
        {
            const string productId = "abc";
            const string productName = "name";
            const decimal productPrice = 10;

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

            var mockedResult = Task.FromResult((OneOf<GetProductByIdResponse, IError>)new GetProductByIdResponse(new Product(ProductId.From(productId))
            {
                Name = ProductName.From(productName),
                Price = ProductPrice.From(productPrice)
            }));

            _mediator.Send(Arg.Any<GetProductByIdRequest>()).Returns(mockedResult);
            _priceCardServiceClient.ActiveAsync(productId, CancellationToken.None).Returns(mockedPriceCardList);

            var request = new GetProductWithPriceCardByIdRequest(ProductId.From(productId));

            var response = await _getProductWithPriceCardByIdRequestHandler
                .Handle(request, CancellationToken.None);

            response.IsT1.Should().BeTrue();
            response.AsT1.Should().NotBeNull();
            response.AsT1.Should().BeOfType<PriceCardNewPriceLessThanZeroError>();
        }

        [Test]
        public async Task Handle_ReturnsProduct_IfProductExists()
        {
            const string productId = "abc";
            const string productName = "name";
            const decimal productPrice = 10;

            var cancellationToken = default(CancellationToken);

            var mockedPriceCardList = Task.FromResult(new PriceCardList
            {
                Items = new List<PriceCard>()
            });

            var mockedResult = Task.FromResult((OneOf<GetProductByIdResponse, IError>)new GetProductByIdResponse(new Product(ProductId.From(productId))
            {
                Name = ProductName.From(productName),
                Price = ProductPrice.From(productPrice)
            }));

            _mediator.Send(Arg.Any<GetProductByIdRequest>()).Returns(mockedResult);
            _priceCardServiceClient.ActiveAsync(productId, cancellationToken).Returns(mockedPriceCardList);

            var request = new GetProductWithPriceCardByIdRequest(ProductId.From(productId));

            var response = await _getProductWithPriceCardByIdRequestHandler
                .Handle(request, cancellationToken);

            response.IsT0.Should().BeTrue();
            response.AsT0.Should().NotBeNull();

            var productWithPriceCard = response.AsT0.ProductWithPriceCard;
            productWithPriceCard.Should().NotBeNull();
            productWithPriceCard.Product.Id.Value.Should().Be(productId);
            productWithPriceCard.Product.Name.Value.Should().Be(productName);
            productWithPriceCard.Product.Name.Value.Should().Be(productName);
            productWithPriceCard.Product.Price.Value.Should().Be(productPrice);
            productWithPriceCard.PriceCard.Should().BeNull();
            productWithPriceCard.FinalPrice.Value.Should().Be(productPrice);
        }

        [Test]
        public async Task Handle_ReturnsProductWithPriceCardPrice_IfProductAndPriceCardExist()
        {
            const string productId = "abc";
            const string productName = "name";
            const decimal productPrice = 10;
            const double priceCardPrice = 8;


            var mockedPriceCardList = Task.FromResult(new PriceCardList()
            {
                Items = new List<PriceCard>
                {
                    new()
                    {
                        NewPrice = priceCardPrice
                    }
                }
            });

            var mockedResult = Task.FromResult((OneOf<GetProductByIdResponse, IError>)new GetProductByIdResponse(new Product(ProductId.From(productId))
            {
                Name = ProductName.From(productName),
                Price = ProductPrice.From(productPrice)
            }));

            _mediator.Send(Arg.Any<GetProductByIdRequest>()).Returns(mockedResult);
            _priceCardServiceClient.ActiveAsync(productId, CancellationToken.None).Returns(mockedPriceCardList);

            var request = new GetProductWithPriceCardByIdRequest(ProductId.From(productId));

            var response = await _getProductWithPriceCardByIdRequestHandler
                .Handle(request, CancellationToken.None);

            response.IsT0.Should().BeTrue();
            response.AsT0.Should().NotBeNull();

            var productWithPriceCard = response.AsT0.ProductWithPriceCard;
            productWithPriceCard.Should().NotBeNull();
            productWithPriceCard.Product.Id.Value.Should().Be(productId);
            productWithPriceCard.Product.Name.Value.Should().Be(productName);
            productWithPriceCard.Product.Price.Value.Should().Be(System.Convert.ToDecimal(productPrice));
            productWithPriceCard.PriceCard.Should().NotBeNull();
            productWithPriceCard.PriceCard?.NewPrice.Value.Should().Be(System.Convert.ToDecimal(priceCardPrice));
            productWithPriceCard.FinalPrice.Value.Should().Be(System.Convert.ToDecimal(priceCardPrice));
        }
    }
}