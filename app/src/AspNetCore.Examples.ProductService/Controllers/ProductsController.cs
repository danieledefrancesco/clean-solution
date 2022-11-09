using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.CreateProductCommand;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.GetProductById;
using AspNetCore.Examples.ProductService.GetProductWithPriceCardById;
using AspNetCore.Examples.ProductService.UpdateProductCommand;
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
        public Task<IActionResult> Get(GetProductByIdRequestDto getProductByIdRequestDto, CancellationToken cancellationToken)
        {
            var getProductByIdRequest = _mapper.Map<GetProductByIdRequest>(getProductByIdRequestDto);
            return MediatorResponse<GetProductByIdRequest, GetProductByIdResponse>(
                getProductByIdRequest,
                response => _mapper.Map<ProductDto>(response), cancellationToken);
        }

        [HttpGet("{id}/with-price-card")]
        public Task<IActionResult> GetWithPriceCard(GetProductWithPriceCardByIdRequestDto getProductWithPriceCardByIdRequestDto, CancellationToken cancellationToken)
        {
            var getProductByIdRequest = _mapper.Map<GetProductWithPriceCardByIdRequest>(getProductWithPriceCardByIdRequestDto);
            return MediatorResponse<GetProductWithPriceCardByIdRequest, GetProductWithPriceCardByIdResponse>(
                getProductByIdRequest,
                response => _mapper.Map<ProductWithPriceCardDto>(response), cancellationToken);
        }
        
        [HttpPatch("{id}")]
        public Task<IActionResult> Update(UpdateProductCommandRequestDto updateProductCommandRequestDto, CancellationToken cancellationToken)
        {
            var updateProductCommandRequest = _mapper.Map<UpdateProductCommandRequest>(updateProductCommandRequestDto);
            return MediatorResponse<UpdateProductCommandRequest, UpdateProductCommandResponse>(
                updateProductCommandRequest,
                response => _mapper.Map<ProductDto>(response), cancellationToken);
        }

        [HttpPost]
        [HttpPut]
        public Task<IActionResult> Insert([FromBody] CreateProductCommandRequestDto createProductRequest, CancellationToken cancellationToken)
        {
            var createProductCommandRequest = _mapper.Map<CreateProductCommandRequest>(createProductRequest);
            return MediatorResponse<CreateProductCommandRequest, CreateProductCommandResponse>(
                createProductCommandRequest,
                response => _mapper.Map<ProductDto>(response), cancellationToken);
        }

    }
}