using AspNetCore.Examples.ProductService.Requests;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.Validators
{
    public class GetProductDtoRequestValidator : AbstractValidator<GetProductDtoRequest>
    {
        public GetProductDtoRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .ValidateProductId();
        }
    }
}