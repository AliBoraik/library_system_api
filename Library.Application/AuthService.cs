using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Library.Domain;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Domain.Models;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Library.Application;

public class AuthService(UserManager<User> userManager, ITokenService tokenService)
    : IAuthService
{
    public async Task<Result<AuthDataResponse, Error>> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null) return new Error(StatusCodes.Status401Unauthorized, ResponseMessage.UnauthorizedAccess);
        if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
            return new Error(StatusCodes.Status401Unauthorized, StringConstants.IncorrectPassword);
        var userRoles = await userManager.GetRolesAsync(user);
        var accessAuthClaims = new List<Claim>
        {
            new(AppClaimTypes.Name, user.UserName!),
            new(AppClaimTypes.Email, user.Email!),
            new(AppClaimTypes.Id, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        accessAuthClaims.AddRange(userRoles.Select(userRole => new Claim(AppClaimTypes.Role, userRole)));
        
        var refreshAuthClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
        };
        
        var generatedAccessToken = tokenService.CreateAccessToken(accessAuthClaims);
        var refreshToken = tokenService.CreateRefreshToken(refreshAuthClaims);

        return new AuthDataResponse
        {
            AccessToken = generatedAccessToken.AccessToken,
            RefreshToken = refreshToken,
            ExpirationTime = generatedAccessToken.ValidTo
        };
    }
    
    public async Task<Result<AuthDataResponse, Error>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var tokenValidationResult = await tokenService.RefreshTokenValidationResult(refreshTokenDto.RefreshToken);
        
        if (!tokenValidationResult.IsValid)
            return new Error(StatusCodes.Status401Unauthorized, "Not Validate refresh token");
        
        var accessTokenValidationResult = await tokenService.AccessTokenValidationResult(refreshTokenDto.AccessToken);
        
        if (!accessTokenValidationResult.IsValid)
            return new Error(StatusCodes.Status401Unauthorized, "Not Validate access token");
        
        var generatedAccessToken = tokenService.CreateAccessToken(accessTokenValidationResult.ClaimsIdentity.Claims.ToList());
        
        return new AuthDataResponse
        {
            AccessToken = generatedAccessToken.AccessToken,
            RefreshToken = refreshTokenDto.RefreshToken,
            ExpirationTime = generatedAccessToken.ValidTo
        };
    }

    public async Task<Result<Ok, Error>> RegisterTeacher(RegisterDto dto)
    {
        var userExists = await userManager.FindByEmailAsync(dto.Email);
        if (userExists != null)
            return new Error(StatusCodes.Status409Conflict, StringConstants.UserAlreadyExists);

        var user = new User
        {
            Email = dto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = dto.Username,
            Teacher = new Teacher()
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, result.Errors.First().Description);
        await userManager.AddToRoleAsync(user, AppRoles.Teacher);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> RegisterStudent(RegisterDto dto)
    {
        var userExists = await userManager.FindByEmailAsync(dto.Email);
        if (userExists != null)
            return new Error(StatusCodes.Status409Conflict, StringConstants.UserAlreadyExists);
        var user = new User
        {
            Email = dto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = dto.Username,
            Student = new Student()
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, result.Errors.First().Description);
        await userManager.AddToRoleAsync(user, AppRoles.Student);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> RegisterAdmin(RegisterDto dto)
    {
        var userExists = await userManager.FindByEmailAsync(dto.Email);
        if (userExists != null)
            return new Error(StatusCodes.Status409Conflict, StringConstants.UserAlreadyExists);
        User user = new()
        {
            Email = dto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = dto.Username
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, result.Errors.First().Description);
        await userManager.AddToRoleAsync(user, AppRoles.Admin);
        return new Ok();
    }
    
    /*private async Task<TokenValidationResult> AccessTokenValidationResult(string? token)
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
        return await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
    }
    
    private async Task< TokenValidationResult> RefreshTokenValidationResult(string? refreshToken)
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
    private JwtSecurityToken CreateAccessToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
            expires: DateTime.Now.AddMinutes(jwtOptions.TokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
    
    private JwtSecurityToken CreateRefreshToken(List<Claim> authClaims)
    {
        
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            expires: DateTime.Now.AddMinutes(jwtOptions.RefreshTokenValidityInDays),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }*/
}