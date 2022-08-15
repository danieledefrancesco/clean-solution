using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    public class GetProductByIdRequestHandler : IAppRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPriceCardServiceClientFactory _priceCardServiceClientFactory;

        public GetProductByIdRequestHandler(IProductRepository productRepository, IPriceCardServiceClientFactory priceCardServiceClientFactory)
        {
            _productRepository = productRepository;
            _priceCardServiceClientFactory = priceCardServiceClientFactory;
        }

        public async Task<OneOf<GetProductByIdResponse, IError>> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.ProductId);
            
            if (product == null)
            {
                return new NotFoundError
                {
                    Message = $"Could not find product {request.ProductId}"
                };
            }

            var priceCardList = await _priceCardServiceClientFactory.Create().ActiveAsync(product.Id, cancellationToken);

            if (!priceCardList.Items.Any())
            {
                product.FinalPrice = product.Price;
                return new GetProductByIdResponse(product);
            }
            var priceCard = priceCardList.Items.First();
            if (priceCard.NewPrice < 0)
            {
                return new PriceCardNewPriceLessThanZeroError
                {
                    Message = $"Price {priceCard.NewPrice} for PriceCard {priceCard.Id} for Product {product.Id} must be greater or equal to 0"
                };
            }
            product.FinalPrice = ProductPrice.From(System.Convert.ToDecimal(priceCard.NewPrice));

            return new GetProductByIdResponse(product);
        }
    }
}