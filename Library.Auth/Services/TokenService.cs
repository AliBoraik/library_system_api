using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Domain.Auth;
using Library.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace Library.Auth.Services;

public class TokenService(JwtOptions jwtOptions) : ITokenService
{
    private readonly  SymmetricSecurityKey _accessSymmetricSecurityKey = new(Encoding.UTF8.GetBytes(jwtOptions.AccessSigningKey));
    private readonly  SymmetricSecurityKey _refreshSymmetricSecurityKey = new(Encoding.UTF8.GetBytes(jwtOptions.RefreshSigningKey));
    
    public GeneratedAccessToken CreateAccessToken(List<Claim> accessAuthClaims)
    {
        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
            expires: DateTime.Now.AddMinutes(jwtOptions.TokenValidityInMinutes),
            claims: accessAuthClaims,
            signingCredentials: new SigningCredentials(_accessSymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );
        return new GeneratedAccessToken
        {
            AccessToken = WriteTokenToString(token),
            ValidTo = ToUnixTimestampSeconds(token.ValidTo)
        };
    }
    

    public string CreateRefreshToken(List<Claim> authClaims)
    {
        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
            expires: DateTime.Now.AddDays(jwtOptions.RefreshTokenValidityInDays),
            claims: authClaims,
            signingCredentials: new SigningCredentials(_refreshSymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );
        return WriteTokenToString(token);
    }

    public async Task<TokenValidationResult> AccessTokenValidationResult(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidIssuer = jwtOptions.Issuer,
            IssuerSigningKey = _accessSymmetricSecurityKey,
            ValidateLifetime = false
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        return await tokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters);
    }

    public async Task<TokenValidationResult> RefreshTokenValidationResult(string refreshToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidIssuer = jwtOptions.Issuer,
            IssuerSigningKey = _refreshSymmetricSecurityKey,
            ValidateLifetime = false,
            LifetimeValidator = (_, expires, _, _) =>  expires != null && expires > DateTime.UtcNow
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        return await tokenHandler.ValidateTokenAsync(refreshToken, tokenValidationParameters);
    }
    private static string WriteTokenToString( JwtSecurityToken jwtSecurityToken)
    {
        return new  JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private static long ToUnixTimestampSeconds(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
    }
}