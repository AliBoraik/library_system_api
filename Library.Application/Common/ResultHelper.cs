using Library.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Library.Application.Common
{
    public static class ResultHelper
    {
        public static ActionResult<T> HandleResult<T>(Result<T, Error> result)
        {
            return result.Match<ActionResult<T>>(
                dto => new OkObjectResult(dto),
                error => new ObjectResult(error) { StatusCode = error.Code }
            );
        }
    }
}