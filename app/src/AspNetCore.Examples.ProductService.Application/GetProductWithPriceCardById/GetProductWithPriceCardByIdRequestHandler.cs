using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.GetProductById;
using AspNetCore.Examples.ProductService.Products;
using AspNetCore.Examples.ProductService.RequestHandlers;
using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdRequestHandler : IAppRequestHandler<GetProductWithPriceCardByIdRequest, GetProductWithPriceCardByIdResponse>
    {
        private readonly IMediator _mediator;
        private readonly PriceCardServiceClient _priceCardServiceClient;

        public GetProductWithPriceCardByIdRequestHandler(IMediator mediator, PriceCardServiceClient priceCardServiceClient)
        {
            _mediator = mediator;
            _priceCardServiceClient = priceCardServiceClient;
        }


        public async Task<OneOf<GetProductWithPriceCardByIdResponse, ErrorBase>> Handle(GetProductWithPriceCardByIdRequest request, CancellationToken cancellationToken)
        {

            var getProductByIdResponse = await  _mediator.Send(new GetProductByIdRequest(request.ProductId), cancellationToken);
            return await getProductByIdResponse.ThrowErrorOrContinueWith(x => GetProductWithPriceCard(x.Product, cancellationToken));
        }

        private async Task<OneOf<GetProductWithPriceCardByIdResponse, ErrorBase>> GetProductWithPriceCard(Product product, CancellationToken cancellationToken)
        {
            var priceCardList = await _priceCardServiceClient.ActiveAsync(product.Id.Value, cancellationToken);

            if (!priceCardList.Items.Any())
            {
                return new GetProductWithPriceCardByIdResponse(product.ApplyPriceCard(null));
            }

            var priceCard = priceCardList.Items.First();
            if (priceCard is { NewPrice: < 0 })
            {
                return new PriceCardNewPriceLessThanZeroError
                {
                    Message =
                        $"Price {priceCard.NewPrice} for PriceCard {priceCard.Id} for Product {product.Id} must be greater or equal to 0"
                };
            }

            var productPriceCard = new ProductPriceCard(priceCard.Id, ProductPrice.From(Convert.ToDecimal(priceCard.NewPrice)));

            return new GetProductWithPriceCardByIdResponse(product.ApplyPriceCard(productPriceCard));
        }
    }
}