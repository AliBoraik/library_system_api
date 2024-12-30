namespace Library.Domain.Constants;

public static class StringConstants
{
    // General error messages
    public const string NotFound = "{0} not found";
    public const string Conflict = "{0} already exists";
    public const string Forbidden = "You do not have permission to {0}";
    public const string Unauthorized = "Unauthorized to {0}";

    // Common error types
    public const string ResourceLocked = "Resource is locked and cannot be modified";
    public const string FileAccessDenied = "You do not have permission to access this file";

    // Common action types
    public const string DeleteAction = "delete";
    public const string CreateAction = "create";
    public const string UpdateAction = "update";
    public const string ViewAction = "view";
    public const string DownloadAction = "download";

    // Internal Server Error message
    public const string InternalServerError = "An unexpected error occurred while processing request";

    // BadRequest message
    public const string BadRequest = "Invalid request: {0}";
}