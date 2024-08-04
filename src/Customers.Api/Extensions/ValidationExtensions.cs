using ErrorOr;
using FluentValidation.Results;

namespace Customers.Api.Extensions;

public static class ValidationExtensions
{
    public static ErrorOr<T> ToErrorOr<T>(this ValidationResult validationResult, T value)
    {
        if (validationResult.IsValid)
        {
            return value;
        }

        var errors = validationResult.Errors
            .Select(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage))
            .ToList();

        return errors;
    }
}
