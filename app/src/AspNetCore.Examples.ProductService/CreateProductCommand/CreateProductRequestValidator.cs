using AspNetCore.Examples.ProductService.Validators;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.CreateProductCommand
{
    public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequestDto>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .ValidateProductId();
            RuleFor(x => x.Name)
                .NotNullOrWhiteSpace();
        }
    }
}