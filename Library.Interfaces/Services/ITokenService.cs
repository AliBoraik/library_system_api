using System.Security.Claims;
using Library.Domain.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Library.Interfaces.Services;

public interface ITokenService
{
    GeneratedAccessToken CreateAccessToken(List<Claim> accessAuthClaims);
    string CreateRefreshToken(List<Claim> authClaims);
    Task<TokenValidationResult> AccessTokenValidationResult(string accessToken);
    Task<TokenValidationResult> RefreshTokenValidationResult(string refreshToken);
}