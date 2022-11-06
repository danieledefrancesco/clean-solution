using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class CreateProductCommandRequestProfile : Profile
    {
        public CreateProductCommandRequestProfile()
        {
            CreateMap<CreateProductRequestDto, CreateProductCommandRequest>()
                .ConstructUsing(dto => new CreateProductCommandRequest(
                    ProductId.From(dto.Id),
                    ProductName.From(dto.Name),
                    ProductPrice.From(dto.Price)));
        }
    }
}