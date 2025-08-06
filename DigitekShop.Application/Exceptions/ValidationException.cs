using FluentValidation.Results;

namespace DigitekShop.Application.Exceptions
{
    public class ApplicationValidationException : Exception
    {
        public ApplicationValidationException() : base()
        {
            Errors = new List<string>();
        }

        public ApplicationValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures.Select(e => e.ErrorMessage).ToList();
        }

        public ApplicationValidationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }

        public ApplicationValidationException(ValidationResult validationResult) : this()
        {
            foreach (var err in validationResult.Errors)
            {
                Errors.Add(err.ErrorMessage);
            }
        }

        public List<string> Errors { get; set; } = new List<string>();
    }
} 