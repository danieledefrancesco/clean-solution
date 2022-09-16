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
            
            
        }
    }
}