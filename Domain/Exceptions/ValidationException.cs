using FluentValidation.Results;

namespace Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public List<ValidationFailure> Errors { get; }

        public ValidationException(string message, List<ValidationFailure> errors) 
            : base(message)
        {
            Errors = errors;
        }
    }
}
