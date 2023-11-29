using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }
    public List<string> Errors { get; }
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        foreach (var failure in failures)
        {
            Errors.Add(failure.ErrorMessage);
        }
    }

    public ValidationException(IEnumerable<string> failures)
        : this()
    {
        foreach (var failure in failures)
        {
            Errors.Add(failure);
        }
    }

}