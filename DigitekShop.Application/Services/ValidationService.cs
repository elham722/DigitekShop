using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DigitekShop.Application.Services
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync<T>(T request);
    }

    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ValidationResult> ValidateAsync<T>(T request)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();
            
            if (validator == null)
            {
                // اگر validator پیدا نشد، موفق در نظر می‌گیریم
                return new ValidationResult();
            }

            return await validator.ValidateAsync(request);
        }
    }
} 