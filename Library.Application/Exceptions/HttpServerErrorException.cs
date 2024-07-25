using System.Net;
using Library.Domain;
using Library.Domain.Constants;

namespace Library.Application.Exceptions;

public class HttpServerErrorException : Exception
{
    public HttpServerErrorException(HttpStatusCode statusCode, string responseMessage)
    {
        StatusCode = statusCode;
        Response = new Response
        {
            StatusText = ResponseStatus.Error,
            Data = responseMessage
        };
    }

    public Response Response { get; }
    public HttpStatusCode StatusCode { get; }
}