using Microsoft.AspNetCore.Mvc;

namespace Library.Domain.Results.Common;

public static class ResultHelper
{
    /// <summary>
    ///     Handles the result of an operation and returns an appropriate HTTP response.
    ///     If the operation is successful, it returns an `OkObjectResult` with the data.
    ///     If the operation fails, it returns an `ObjectResult` with the error and sets the status code to the error's code.
    /// </summary>
    /// <param name="result">The result of the operation, which can either contain data or an error.</param>
    public static ActionResult<T> HandleResult<T>(Result<T, Error> result)
    {
        return result.Match<ActionResult<T>>(
            dto => new OkObjectResult(dto),
            error => new ObjectResult(error) { StatusCode = error.Code }
        );
    }

    /// <summary>
    ///     Returns a successful result with an empty `Ok` object.
    ///     This is used to represent a successful operation that does not return any data.
    /// </summary>
    public static Result<Ok, Error> Ok()
    {
        return Result<Ok, Error>.Ok(new Ok());
    }
}