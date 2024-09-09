namespace Library.Domain.Auth;

public class AuthDataResponse 
{
    public string AccessToken { get; set; } = null!;
    public DateTime Expiration { get; set; }
}