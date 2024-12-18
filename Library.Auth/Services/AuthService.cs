using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Library.Domain;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Library.Auth.Services;

public class AuthService(UserManager<User> userManager, ITokenService tokenService , AppDbContext  context)
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
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        accessAuthClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var refreshAuthClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

        var accessAuthClaims = accessTokenValidationResult.ClaimsIdentity.Claims.ToList();

        var generatedAccessToken = tokenService.CreateAccessToken(accessAuthClaims);

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
        };
        
        var createUserResult = await userManager.CreateAsync(user, dto.Password);
        if (!createUserResult.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, createUserResult.Errors.First().Description);
        // Create Teacher
        var teacher = new Teacher
        {
            Id = user.Id,
        };
        await context.Teachers.AddAsync(teacher);
        await context.SaveChangesAsync();
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
            UserName = dto.Username
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, result.Errors.First().Description);
        // Create Student
        var student = new Student
        {
            Id = user.Id,
        };
        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();
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
}