using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    public class GetProductByIdRequestHandler : IAppRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdRequestHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<OneOf<GetProductByIdResponse, IError>> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.ProductId);
            
            if (product == null)
            {
                return new NotFoundError()
                {
                    Message = $"Could not find product {request.ProductId}"
                };
            }

            return new GetProductByIdResponse(product);
        }
    }
}