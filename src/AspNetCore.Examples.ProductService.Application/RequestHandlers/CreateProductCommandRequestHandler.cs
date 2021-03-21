using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    public class CreateProductCommandRequestHandler : IAppRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandRequestHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<OneOf<CreateProductCommandResponse, IError>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var productToCreate = request.ProductToCreate;
            var productId = productToCreate.Id;
            var productExists = await _productRepository.ExistsById(productId);

            if (productExists)
            {
                return new AlreadyExistsError()
                {
                    Message = $"The product {productId} already exists"
                };
            }

            await _productRepository.Insert(productToCreate);
            return new CreateProductCommandResponse(productToCreate);
        }
    }
}