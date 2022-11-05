using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    public sealed class GetProductWithPriceCardByIdRequestHandler : IAppRequestHandler<GetProductWithPriceCardByIdRequest, GetProductWithPriceCardByIdResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly PriceCardServiceClient _priceCardServiceClient;

        public GetProductWithPriceCardByIdRequestHandler(IProductRepository productRepository, PriceCardServiceClient priceCardServiceClient)
        {
            _productRepository = productRepository;
            _priceCardServiceClient = priceCardServiceClient;
        }

        public async Task<OneOf<GetProductWithPriceCardByIdResponse, IError>> Handle(GetProductWithPriceCardByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.ProductId);
            
            if (product == null)
            {
                return new NotFoundError
                {
                    Message = $"Could not find product {request.ProductId}"
                };
            }
            
            var priceCardList = await _priceCardServiceClient.ActiveAsync(product.Id, cancellationToken);
            var priceCard = priceCardList.Items.FirstOrDefault();
            if (priceCard is { NewPrice: < 0 })
            {
                return new PriceCardNewPriceLessThanZeroError
                {
                    Message = $"Price {priceCard.NewPrice} for PriceCard {priceCard.Id} for Product {product.Id} must be greater or equal to 0"
                };

            }
            ProductPriceCard? productPriceCard = priceCard == null
                ? null : new ProductPriceCard(priceCard.Id, ProductPrice.From(Convert.ToDecimal(priceCard.NewPrice)));

            return new GetProductWithPriceCardByIdResponse(product.ApplyPriceCard(productPriceCard));
        }
    }
}