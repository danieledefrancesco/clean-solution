using AspNetCore.Examples.ProductService.Products;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class OnProductUpdatedProfile : Profile
    {
        public OnProductUpdatedProfile()
        {
            CreateMap<OnProductUpdated, OnProductUpdatedEventDto>()
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