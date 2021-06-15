using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : AppControllerBase
    {
        private readonly IMapper _mapper;

        public ProductsController(
            IErrorHandlerFactory errorHandlerFactory,
            IMediator mediator,
            IMapper mapper) : base(errorHandlerFactory, mediator)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> Get([FromRoute] GetProductDtoRequest getProductDtoRequest)
        {
            var getProductByIdRequest = _mapper.Map<GetProductByIdRequest>(getProductDtoRequest);
            return MediatorResponse<GetProductByIdRequest, GetProductByIdResponse>(
                getProductByIdRequest,
                response => _mapper.Map<ProductDto>(response.Product));
        }

        [HttpPost]
        [HttpPut]
        public Task<IActionResult> Insert([FromBody] ProductDto product)
        {
            var domainProduct = _mapper.Map<Product>(product);
            var createProductCommandRequest = new CreateProductCommandRequest(domainProduct);
            return MediatorResponse<CreateProductCommandRequest, CreateProductCommandResponse>(
                createProductCommandRequest,
                response => _mapper.Map<ProductDto>(response.CreatedProduct));
        }

    }
}