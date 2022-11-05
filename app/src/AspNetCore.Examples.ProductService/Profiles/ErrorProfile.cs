using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class ErrorProfile : Profile
    {
        public ErrorProfile()
        {
            CreateMap<IError, ErrorDto>();
        }
    }
}