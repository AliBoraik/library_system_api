using System.Net;

namespace Library.Application.Exceptions;

public class UnauthorizedException : HttpServerErrorException
{
    public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message)
    {
    }
}