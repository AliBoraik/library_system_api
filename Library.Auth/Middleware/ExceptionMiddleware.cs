using Library.Domain;
using Library.Domain.Constants;

namespace Library.Auth.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during executing {Context}", context.Request.Path.Value);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new Error
            (
                StatusCodes.Status500InternalServerError,
                StringConstants.InternalServerError
            ));
        }
    }
}