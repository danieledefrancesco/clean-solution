using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;
using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
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


        public async Task<OneOf<GetProductWithPriceCardByIdResponse, IError>> Handle(GetProductWithPriceCardByIdRequest request, CancellationToken cancellationToken)
        {

            var getProductByIdResponse = await  _mediator.Send(new GetProductByIdRequest(request.ProductId), cancellationToken);
            if (getProductByIdResponse.IsT1) return (ErrorBase) getProductByIdResponse.AsT1;
            return await GetProductWithPriceCard(getProductByIdResponse.AsT0.Product, cancellationToken);
        }

        private async Task<OneOf<GetProductWithPriceCardByIdResponse, IError>> GetProductWithPriceCard(Product product, CancellationToken cancellationToken)
        {
            var priceCardList = await _priceCardServiceClient.ActiveAsync(product.Id, cancellationToken);

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