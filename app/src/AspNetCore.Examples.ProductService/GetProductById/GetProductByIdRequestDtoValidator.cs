using AspNetCore.Examples.ProductService.Validators;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductByIdRequestDtoValidator : AbstractValidator<GetProductByIdRequestDto>
    {
        public GetProductByIdRequestDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .ValidateProductId();
        }
    }
}