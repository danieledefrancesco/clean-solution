using System.Threading.Tasks;
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
    public sealed class ProductsController : AppControllerBase
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
        public Task<IActionResult> Get([FromRoute] GetProductWithPriceCardByIdRequestDto getProductWithPriceCardByIdRequestDto)
        {
            var getProductByIdRequest = _mapper.Map<GetProductWithPriceCardByIdRequest>(getProductWithPriceCardByIdRequestDto);
            return MediatorResponse<GetProductWithPriceCardByIdRequest, GetProductWithPriceCardByIdResponse>(
                getProductByIdRequest,
                response => _mapper.Map<ProductDto>(response));
        }

        [HttpPost]
        [HttpPut]
        public Task<IActionResult> Insert([FromBody] CreateProductRequestDto createProductRequest)
        {
            var createProductCommandRequest = _mapper.Map<CreateProductCommandRequest>(createProductRequest);
            return MediatorResponse<CreateProductCommandRequest, CreateProductCommandResponse>(
                createProductCommandRequest,
                response => _mapper.Map<ProductDto>(response));
        }

    }
}