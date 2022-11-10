using AspNetCore.Examples.ProductService.Products;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.CreateProductCommand
{
    public sealed class CreateProductCommandRequestProfile : Profile
    {
        public CreateProductCommandRequestProfile()
        {
            CreateMap<CreateProductCommandRequestDto, CreateProductCommandRequest>()
                .ConstructUsing(dto => new CreateProductCommandRequest(
                    ProductId.From(dto.Id),
                    ProductName.From(dto.Name),
                    ProductPrice.From(dto.Price)));
        }
    }
}