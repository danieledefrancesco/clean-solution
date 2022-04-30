using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(
                    productDto => productDto.Name,
                    opt => opt.MapFrom(dest => dest.Name.Value))
                .ReverseMap()
                .ForMember(
                    product => product.Name,
                    opt => opt.MapFrom(dest => ProductName.From(dest.Name)));
            
            CreateMap<GetProductByIdRequest, GetProductDtoRequest>().ReverseMap();
        }
    }
}