using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Attributes;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.GetProductById;
using AspNetCore.Examples.ProductService.Products;
using AspNetCore.Examples.ProductService.RequestHandlers;
using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    [Transaction]
    public sealed class UpdateProductCommandRequestHandler: IAppRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;

        public UpdateProductCommandRequestHandler(IProductRepository productRepository, IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        public async Task<OneOf<UpdateProductCommandResponse, ErrorBase>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var getProductByIdResponse =
                await _mediator.Send(new GetProductByIdRequest(request.ProductId), cancellationToken);
            return await getProductByIdResponse.ThrowErrorOrContinueWith(async response =>
            {
                var product = response.Product;
                product.Update(request.ProductName, request.ProductPrice);
                await _productRepository.Update(product);
                return new UpdateProductCommandResponse(product);
            });
        }
    }
}