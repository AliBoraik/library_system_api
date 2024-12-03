using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Domain.Auth;
using Library.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace Library.Application;

public class TokenService(JwtOptions jwtOptions) : ITokenService
{
    private readonly  SymmetricSecurityKey _symmetricSecurityKey = new(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
    
    public GeneratedAccessToken CreateAccessToken(List<Claim> accessAuthClaims)
    {
        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
            expires: DateTime.Now.AddMinutes(jwtOptions.TokenValidityInMinutes),
            claims: accessAuthClaims,
            signingCredentials: new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
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
            expires: DateTime.Now.AddMinutes(jwtOptions.RefreshTokenValidityInDays),
            claims: authClaims,
            signingCredentials: new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
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
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
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