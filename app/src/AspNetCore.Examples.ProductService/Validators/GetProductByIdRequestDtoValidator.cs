using AspNetCore.Examples.ProductService.DataTransferObjects;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.Validators
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