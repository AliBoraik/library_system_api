using Library.Domain.Constants;

namespace Library.Domain.Results;

public static class Errors
{
    public static Error NotFound(string resource)
    {
        return new Error(404, string.Format(StringConstants.NotFound, resource));
    }

    public static Error Conflict(string resource)
    {
        return new Error(409, string.Format(StringConstants.Conflict, resource));
    }

    public static Error Forbidden(string action)
    {
        return new Error(403, string.Format(StringConstants.Forbidden, action));
    }

    public static Error Unauthorized(string action)
    {
        return new Error(401, string.Format(StringConstants.Unauthorized, action));
    }

    public static Error ResourceLocked()
    {
        return new Error(423, StringConstants.ResourceLocked);
    }

    public static Error FileAccessDenied()
    {
        return new Error(403, StringConstants.FileAccessDenied);
    }

    public static Error InternalServerError()
    {
        return new Error(500, StringConstants.InternalServerError);
    }

    public static Error BadRequest(string message)
    {
        return new Error(400, message);
    }
}