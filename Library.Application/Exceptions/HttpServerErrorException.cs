using System.Net;
using Library.Domain;
using Library.Domain.Constants;

namespace Library.Application.Exceptions;

public class HttpServerErrorException(HttpStatusCode statusCode, string responseMessage) : Exception
{
    public Response Response { get; } = new()
    {
        StatusText = ResponseStatus.Error,
        Message = responseMessage
    };

    public HttpStatusCode StatusCode { get; } = statusCode;
}