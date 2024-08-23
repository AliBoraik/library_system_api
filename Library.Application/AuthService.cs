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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ok = Library.Domain.Ok;

namespace Library.Application;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<Result<AuthDataResponse, Error>> Login(LoginModelDto loginModelDto)
    {
        var user = await _userManager.FindByNameAsync(loginModelDto.Username);
        if (user == null) return new Error(StatusCodes.Status401Unauthorized,ResponseMessage.UnauthorizedAccess);
        if (!await _userManager.CheckPasswordAsync(user, loginModelDto.Password)) 
            return new Error(StatusCodes.Status401Unauthorized,StringConstants.IncorrectPassword); 
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
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
        var userExists = await _userManager.FindByNameAsync(modelDto.Username);
         if (userExists != null)
             return new Error(StatusCodes.Status409Conflict,StringConstants.UserAlreadyExists);
        ApplicationUser user = new()
        {
            Email = modelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDto.Username
        };
        var result = await _userManager.CreateAsync(user, modelDto.Password); 
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError,result.Errors.First().Description);
        await _userManager.AddToRoleAsync(user, AppRoles.Teacher);
        return new Ok();
        
    }

    public async Task<Result<Ok, Error>> RegisterAdmin(RegisterModelDto modelDto)
    {
        var userExists = await _userManager.FindByNameAsync(modelDto.Username);
        if (userExists != null)
            return new Error(StatusCodes.Status409Conflict,StringConstants.UserAlreadyExists);
        ApplicationUser user = new()
        {
            Email = modelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDto.Username
        };
        var result = await _userManager.CreateAsync(user, modelDto.Password); 
        if (!result.Succeeded)
            return new Error(StatusCodes.Status500InternalServerError,result.Errors.First().Description);
        await _userManager.AddToRoleAsync(user, AppRoles.Admin);
        return new Ok();
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
}