using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<GetProductWithPriceCardByIdRequestDto, GetProductWithPriceCardByIdRequest>()
                .ConstructUsing(dto => new GetProductWithPriceCardByIdRequest(ProductId.From(dto.ProductId)));
            
            CreateMap<GetProductWithPriceCardByIdResponse, ProductDto>()
                .ForMember(dto => dto.Id,
                    opt =>
                        opt.MapFrom(p => p.ProductWithPriceCard.Product.Id.Value))
                .ForMember(dto => dto.Name,
                    opt =>
                        opt.MapFrom(p => p.ProductWithPriceCard.Product.Name.Value))
                .ForMember(dto => dto.Price,
                    opt =>
                        opt.MapFrom(p => p.ProductWithPriceCard.Product.Price.Value))
                .ForMember(dto => dto.FinalPrice,
                    opt =>
                        opt.MapFrom(p => p.ProductWithPriceCard.FinalPrice.Value));

            CreateMap<CreateProductRequestDto, CreateProductCommandRequest>()
                .ConstructUsing(dto => new CreateProductCommandRequest(
                    ProductId.From(dto.Id),
                    ProductName.From(dto.Name),
                    ProductPrice.From(dto.Price)));

            CreateMap<CreateProductCommandResponse, ProductDto>()
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(response => response.CreatedProduct.Id.Value))
                .ForMember(dto => dto.Name,
                    opt => opt.MapFrom(response => response.CreatedProduct.Name.Value))
                .ForMember(dto => dto.Price,
                    opt => opt.MapFrom(response => response.CreatedProduct.Price.Value))
                .ForMember(dto => dto.FinalPrice,
                    opt => opt.MapFrom(response => response.CreatedProduct.Price.Value));

            CreateMap<OnProductCreated, OnProductCreatedEventDto>()
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(e => e.Id))
                .ForMember(dto => dto.ProductId,
                    opt => opt.MapFrom(e => e.ProductId.Value))
                .ForMember(dto => dto.ProductName,
                    opt => opt.MapFrom(e => e.ProductName.Value))
                .ForMember(dto => dto.ProductPrice,
                    opt => opt.MapFrom(e => e.ProductPrice.Value));

        }
    }
}