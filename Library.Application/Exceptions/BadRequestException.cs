using System.Net;

namespace Library.Application.Exceptions;

public class BadRequestException : HttpServerErrorException
{
    public BadRequestException(string message) : base(HttpStatusCode.BadRequest,  message)
    {
    }
}