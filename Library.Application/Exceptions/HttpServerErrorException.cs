using System.Net;
using Library.Domain;
using Library.Domain.Constants;

namespace Library.Application.Exceptions;

public class HttpServerErrorException : Exception
{
    public Response Response { get; }
    public HttpStatusCode StatusCode { get; }

    protected HttpServerErrorException(HttpStatusCode statusCode ,  string responseMessage)
    {
        StatusCode = statusCode;
        Response = new Response
        {
            StatusText = ResponseStatus.Error,
            Message = responseMessage
        };
    }
}