using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Extensions;
public static class ErrorOrExtensions
{
    public static IResult ToMinimalApiResult<T>(this ErrorOr<T> errorOr)
    {
        if (errorOr.IsError)
        {
            var firstError = errorOr.Errors.First();

            if (firstError.Type == ErrorType.Validation)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    Detail = "See the errors property for details."
                };

                foreach (var error in errorOr.Errors)
                {
                    validationProblemDetails.Errors.Add(error.Code, new[] { error.Description });
                }

                return Results.BadRequest(validationProblemDetails);
            }

            var problemDetails = new ProblemDetails
            {
                Status = firstError.Type switch
                {
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                },
                Title = firstError.Type switch
                {
                    ErrorType.Conflict => "Conflict occurred",
                    ErrorType.NotFound => "Resource not found",
                    _ => "An unexpected error occurred"
                },
                Detail = string.Join("; ", errorOr.Errors.Select(e => e.Description))            
            };

            return Results.Problem(problemDetails);
        }

        if (typeof(T) == typeof(Deleted))
        {
            return Results.NoContent();
        }

        return Results.Ok(errorOr.Value);
    }
}