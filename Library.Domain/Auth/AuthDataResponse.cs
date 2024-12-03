namespace Library.Domain.Auth;

public class AuthDataResponse
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; } 
    public required long ExpirationTime { get; set; }
}