using System.Net;

namespace Library.Api.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            ApplicationException => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
            KeyNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, exception.Message),
            UnauthorizedAccessException => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
        };
        if (response.StatusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "error during executing {Context}", context.Request.Path.Value);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;

        await context.Response.WriteAsJsonAsync(response);
    }
}