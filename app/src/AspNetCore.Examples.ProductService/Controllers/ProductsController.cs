using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.CreateProductCommand;
using AspNetCore.Examples.ProductService.DataTransferObjects;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.GetProductById;
using AspNetCore.Examples.ProductService.GetProductWithPriceCardById;
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
        public Task<IActionResult> Get([FromRoute] GetProductByIdRequestDto getProductByIdRequestDto)
        {
            var getProductByIdRequest = _mapper.Map<GetProductByIdRequest>(getProductByIdRequestDto);
            return MediatorResponse<GetProductByIdRequest, GetProductByIdResponse>(
                getProductByIdRequest,
                response => _mapper.Map<ProductDto>(response));
        }

        [HttpGet("{id}/with-price-card")]
        public Task<IActionResult> GetWithPriceCard([FromRoute] GetProductWithPriceCardByIdRequestDto getProductWithPriceCardByIdRequestDto)
        {
            var getProductByIdRequest = _mapper.Map<GetProductWithPriceCardByIdRequest>(getProductWithPriceCardByIdRequestDto);
            return MediatorResponse<GetProductWithPriceCardByIdRequest, GetProductWithPriceCardByIdResponse>(
                getProductByIdRequest,
                response => _mapper.Map<ProductWithPriceCardDto>(response));
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