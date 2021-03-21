using FluentValidation;

namespace AspNetCore.Examples.ProductService.Validators
{
    public static class CommonValidationExtensions
    {
        public static void NotNullOrWhiteSpace<T>(this IRuleBuilderInitial<T, string> rule)
        {
            rule.Must(x => !string.IsNullOrWhiteSpace(x));
        }
    }
}