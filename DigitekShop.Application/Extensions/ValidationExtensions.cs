using FluentValidation;
using DigitekShop.Application.Exceptions;
using FluentValidation.Results;

namespace DigitekShop.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static void ValidateAndThrow<T>(this IValidator<T> validator, T instance)
        {
            var result = validator.Validate(instance);
            if (!result.IsValid)
            {
                throw new ApplicationValidationException(result);
            }
        }

        public static void ValidateAndThrow<T>(this IValidator<T> validator, T instance, string message)
        {
            var result = validator.Validate(instance);
            if (!result.IsValid)
            {
                throw new ApplicationValidationException(message);
            }
        }

        public static ValidationResult ValidateWithResult<T>(this IValidator<T> validator, T instance)
        {
            return validator.Validate(instance);
        }

        public static bool IsValid<T>(this IValidator<T> validator, T instance)
        {
            return validator.Validate(instance).IsValid;
        }
    }
} 