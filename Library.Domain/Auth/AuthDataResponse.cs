namespace Library.Domain.Auth;

public class AuthDataResponse
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
}