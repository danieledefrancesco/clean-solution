using AspNetCore.Examples.ProductService.Events;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class OnProductCreatedProfile : Profile
    {
        public OnProductCreatedProfile()
        {
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