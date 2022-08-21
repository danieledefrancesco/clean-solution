using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Attributes;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.Handlers;
using AspNetCore.Examples.ProductService.Repositories;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    [Transaction]
    public class CreateProductCommandRequestHandler : IAppRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventHandler _eventHandler;

        public CreateProductCommandRequestHandler(IProductRepository productRepository, IEventHandler eventHandler)
        {
            _productRepository = productRepository;
            _eventHandler = eventHandler;
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
            await _eventHandler.RaiseEvent(new OnProductCreated
            {
                CreatedProduct = productToCreate
            });
            return new CreateProductCommandResponse(productToCreate);
        }
    }
}