using AspNetCore.Examples.ProductService.Products;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class PriceCardProfile : Profile
    {
        public PriceCardProfile()
        {
            CreateMap<ProductPriceCard, PriceCardDto>()
                .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(priceCard => priceCard.PriceCardId))
                .ForMember(dto => dto.NewPrice,
                    opt => opt.MapFrom(priceCard => priceCard.NewPrice.Value));
        }
    }
}