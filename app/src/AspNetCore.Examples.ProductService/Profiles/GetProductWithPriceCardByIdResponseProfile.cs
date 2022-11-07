using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public class GetProductWithPriceCardByIdResponseProfile : Profile
    {
        public GetProductWithPriceCardByIdResponseProfile()
        {
            CreateMap<GetProductWithPriceCardByIdResponse, ProductWithPriceCardDto>()
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
                        opt.MapFrom(p => p.ProductWithPriceCard.FinalPrice.Value))
                .ForMember(dto => dto.PriceCard,
                    opt => opt.MapFrom(p=> p.ProductWithPriceCard.PriceCard));
        }
    }
}