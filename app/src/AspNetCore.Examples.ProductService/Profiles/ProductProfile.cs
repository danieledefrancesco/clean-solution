using System;
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
                .ForMember(
                    productDto => productDto.Price,
                    opt => opt.MapFrom(dest => dest.Price.Value))
                .ForMember(
                    productDto => productDto.FinalPrice,
                    opt => opt.MapFrom(dest => dest.FinalPrice.Value));
            CreateMap<CreateProductRequestDto, Product>()
                .ForMember(
                    product => product.Name,
                    opt => opt.MapFrom(dest => ProductName.From(dest.Name)))
                .ForMember(product => product.Price,
                    opt => opt.MapFrom(dest => ProductPrice.From(dest.Price)))
                .ForMember(product => product.FinalPrice, 
                    opt => opt.MapFrom<ProductPrice>(dest => default))
                .ForMember(product => product.LastModifiedAt, 
                    opt => opt.MapFrom<DateTime>(dest => default))
                .ForMember(product => product.CreatedAt, 
                    opt => opt.MapFrom<DateTime>(dest => default));
            
            CreateMap<GetProductByIdRequest, GetProductDtoRequest>().ReverseMap();
        }
    }
}