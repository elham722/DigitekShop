using FluentValidation.Results;

namespace DigitekShop.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
            Errors = new List<string>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures.Select(e => e.ErrorMessage).ToList();
        }

        public ValidationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(ValidationResult validationResult) : this()
        {
            foreach (var err in validationResult.Errors)
            {
                Errors.Add(err.ErrorMessage);
            }
        }

        public List<string> Errors { get; set; } = new List<string>();
    }
} 