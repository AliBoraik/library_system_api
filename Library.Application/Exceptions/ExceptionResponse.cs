using Library.Domain;

namespace Library.Application.Exceptions;

public class ExceptionResponse : Exception
{
    public Response Response { get; set; }
    public int StatusCodes { get; }
    
    public ExceptionResponse(int statusCodes ,  Response response)
    {
        Response = response;
        StatusCodes = statusCodes;
    }
}