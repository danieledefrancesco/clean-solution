using AspNetCore.Examples.ProductService.Validators;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.CreateProductCommand
{
    public sealed class CreateProductCommandRequestDtoValidator : AbstractValidator<CreateProductCommandRequestDto>
    {
        public CreateProductCommandRequestDtoValidator()
        {
            RuleFor(x => x.Id)
                .ValidateProductId();
            RuleFor(x => x.Name)
                .NotNullOrWhiteSpace();
        }
    }
}