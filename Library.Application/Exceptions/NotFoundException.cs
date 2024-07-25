using System.Net;

namespace Library.Application.Exceptions;

public class NotFoundException : HttpServerErrorException
{
    public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
    {
    }
}