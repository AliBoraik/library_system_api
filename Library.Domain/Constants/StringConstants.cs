namespace Library.Domain.Constants;

public static class StringConstants
{
    public const string UserCreationFailed = "User creation failed! Please check user details and try again.";
    public const string UserCreatedSuccessfully = "User created successfully!";
    public const string UserAlreadyExists = "User already exists!";
    public const string UserIdMissing = "userId is missing in token.";
    public const string UserIdNotCorrect = "userId type is not guid";
    public const string IncorrectPassword = "Uncorrected password";
    public const string InternalServerError = "Internal server error. Please retry later.";

    public const string IncorrectRequiredPasswordErrorMessage =
        "Password must contain at least one uppercase letter, one lowercase letter, one non-alphabetic character, and be at least 8 characters long.";
}