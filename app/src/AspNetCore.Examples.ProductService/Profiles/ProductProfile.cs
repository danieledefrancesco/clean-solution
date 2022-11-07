using AspNetCore.Examples.ProductService.Entities;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(response => response.Id.Value))
                .ForMember(dto => dto.Name,
                    opt => opt.MapFrom(response => response.Name.Value))
                .ForMember(dto => dto.Price,
                    opt => opt.MapFrom(response => response.Price.Value));
        }
    }
}