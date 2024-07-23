using System.Net;

namespace Library.Api.Exceptions;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);