using System;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public class CreateProductRequestProfile : Profile
    {
        public CreateProductRequestProfile()
        {
            CreateMap<CreateProductRequestDto, CreateProductCommandRequest>()
                .ConstructUsing(dto => new CreateProductCommandRequest(new Product
                {
                    Id = dto.Id,
                    Name = ProductName.From(dto.Name),
                    Price = ProductPrice.From(dto.Price)
                }));
        }
    }
}