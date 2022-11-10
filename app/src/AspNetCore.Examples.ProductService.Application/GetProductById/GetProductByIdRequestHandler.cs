using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Products;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.RequestHandlers;
using OneOf;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductByIdRequestHandler : IAppRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdRequestHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<OneOf<GetProductByIdResponse, ErrorBase>> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.ProductId);
            
            if (product == null)
            {
                return new NotFoundError
                {
                    Message = $"Could not find product {request.ProductId}"
                };
            }

            return new GetProductByIdResponse(product);
        }
    }
}