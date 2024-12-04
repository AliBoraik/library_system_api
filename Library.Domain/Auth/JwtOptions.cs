namespace Library.Domain.Auth;

public record JwtOptions(
    string Issuer,
    string Audience,
    string AccessSigningKey,
    string RefreshSigningKey,
    int TokenValidityInMinutes,
    int RefreshTokenValidityInDays
);