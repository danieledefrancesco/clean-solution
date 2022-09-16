using AspNetCore.Examples.ProductService.Requests;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public class GetProductRequestProfile: Profile
    {
        public GetProductRequestProfile()
        {
            CreateMap<GetProductRequestDto, GetProductByIdRequest>()
                .ConstructUsing(dto => new GetProductByIdRequest(dto.ProductId));
        }
    }
}