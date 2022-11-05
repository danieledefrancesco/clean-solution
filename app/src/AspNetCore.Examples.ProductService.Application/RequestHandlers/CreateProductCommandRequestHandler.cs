using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Attributes;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    [Transaction]
    public sealed class CreateProductCommandRequestHandler : IAppRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductsFactory _productsFactory;

        public CreateProductCommandRequestHandler(IProductRepository productRepository, IProductsFactory productsFactory)
        {
            _productRepository = productRepository;
            _productsFactory = productsFactory;
        }

        public async Task<OneOf<CreateProductCommandResponse, IError>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {;
            var productExists = await _productRepository.ExistsById(request.ProductId);
            if (productExists)
            {
                return new AlreadyExistsError
                {
                    Message = $"The product {request.ProductId} already exists"
                };
            }
            
            var productToCreate = _productsFactory.CreateProduct(request.ProductId, p =>
            {
                p.Name = request.ProductName;
                p.Price = request.ProductPrice;
            });

            await _productRepository.Insert(productToCreate);

            return new CreateProductCommandResponse(productToCreate);
        }
    }
}