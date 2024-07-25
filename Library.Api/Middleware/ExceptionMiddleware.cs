using Library.Application.Exceptions;
using Library.Domain;

namespace Library.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpServerErrorException exceptionResponse)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exceptionResponse.StatusCode;
            await context.Response.WriteAsJsonAsync(exceptionResponse.Response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error during executing {Context}", context.Request.Path.Value);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusText = "Error",
                Data = "Internal server error. Please retry later."
            });
        }
    }
}