using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class CreateProductCommandResponseProfile : Profile
    {
        public CreateProductCommandResponseProfile()
        {
            CreateMap<CreateProductCommandResponse, ProductDto>()
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(response => response.CreatedProduct.Id.Value))
                .ForMember(dto => dto.Name,
                    opt => opt.MapFrom(response => response.CreatedProduct.Name.Value))
                .ForMember(dto => dto.Price,
                    opt => opt.MapFrom(response => response.CreatedProduct.Price.Value))
                .ForMember(dto => dto.FinalPrice,
                    opt => opt.MapFrom(response => response.CreatedProduct.Price.Value));
        }
    }
}