using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Domain;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Domain.Models;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace Library.Application;

public class AuthService(UserManager<User> userManager , JwtOptions jwtOptions )
    : IAuthService
{
    public async Task<Result<AuthDataResponse, Error>> Login(LoginModelDto loginModelDto)
    {
        var user = await userManager.FindByEmailAsync(loginModelDto.Email);
        if (user == null) return new Error(StatusCodes.Status401Unauthorized, ResponseMessage.UnauthorizedAccess);
        if (!await userManager.CheckPasswordAsync(user, loginModelDto.Password))
            return new Error(StatusCodes.Status401Unauthorized, StringConstants.IncorrectPassword);
        var userRoles = await userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email , user.Email!),
            new(ClaimTypes.NameIdentifier,user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        var accessToken = GetToken(authClaims);
       
        return new AuthDataResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            Expiration = accessToken.ValidTo
        };
    }

    public async Task<Result<Ok, Error>> RegisterTeacher(RegisterModelDto modelDto)
    {
        var userExists = await userManager.FindByEmailAsync(modelDto.Email);
        if (userExists != null)
            return new Error(StatusCodes.Status409Conflict, StringConstants.UserAlreadyExists);
        var user = new Teacher
        {
            Email = modelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDto.Username
        };
        var result = await userManager.CreateAsync(user, modelDto.Password);
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, result.Errors.First().Description);
        await userManager.AddToRoleAsync(user, AppRoles.Teacher);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> RegisterStudent(RegisterModelDto modelDto)
    {
        var userExists = await userManager.FindByEmailAsync(modelDto.Email);
        if (userExists != null)
            return new Error(StatusCodes.Status409Conflict, StringConstants.UserAlreadyExists);
        var user = new Student
        {
            Email = modelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDto.Username
        };
        var result = await userManager.CreateAsync(user, modelDto.Password);
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, result.Errors.First().Description);
        await userManager.AddToRoleAsync(user, AppRoles.Student);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> RegisterAdmin(RegisterModelDto modelDto)
    {
        var userExists = await userManager.FindByEmailAsync(modelDto.Email);
        if (userExists != null)
            return new Error(StatusCodes.Status409Conflict, StringConstants.UserAlreadyExists);
        User user = new()
        {
            Email = modelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDto.Username
        };
        var result = await userManager.CreateAsync(user, modelDto.Password);
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError, result.Errors.First().Description);
        await userManager.AddToRoleAsync(user, AppRoles.Admin);
        return new Ok();
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
            expires: DateTime.Now.AddSeconds(jwtOptions.ExpirationSeconds),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
}