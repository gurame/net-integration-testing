using ErrorOr;

namespace Customers.Api.Extensions;

public static class ErrorOrExtensions
{
    public static IResult ToMinimalApiResult<T>(this ErrorOr<T> errorOr)
    {
        if (errorOr.IsError)
        {
            var firstError = errorOr.Errors.First();

            return firstError.Type switch
            {
                ErrorType.Validation => Results.BadRequest(new { Errors = errorOr.Errors.Select(e => e.Description) }),
                ErrorType.Conflict => Results.Conflict(new { Errors = errorOr.Errors.Select(e => e.Description) }),
                ErrorType.NotFound => Results.NotFound(new { Errors = errorOr.Errors.Select(e => e.Description) }),
                _ => Results.Problem(string.Join(", ", errorOr.Errors.Select(e => e.Description)))
            };
        }

        if (typeof(T) == typeof(Deleted))
        {
            return Results.NoContent();
        }

        return Results.Ok(errorOr.Value);
    }
}
