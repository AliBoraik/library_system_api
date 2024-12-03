namespace Library.Domain.Auth;

public record JwtOptions(
    string Issuer,
    string Audience,
    string SigningKey,
    int TokenValidityInMinutes,
    int RefreshTokenValidityInDays
);