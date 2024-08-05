using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Library.Application.Exceptions;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Domain.Models;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<AuthDataResponse> Login(LoginModelDto loginModelDto)
    {
        var user = await _userManager.FindByNameAsync(loginModelDto.Username);
        if (user == null) throw new UnauthorizedException(ResponseMessage.UnauthorizedAccess);
        if (!await _userManager.CheckPasswordAsync(user, loginModelDto.Password))
            throw new UnauthorizedException(StringConstants.IncorrectPassword);
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

    public async Task RegisterTeacher(RegisterModelDto modelDto)
    {
        var userExists = await _userManager.FindByNameAsync(modelDto.Username);
        if (userExists != null)
            throw new BadRequestException(StringConstants.UserAlreadyExists);
        ApplicationUser user = new()
        {
            Email = modelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDto.Username
        };
        var result = await _userManager.CreateAsync(user, modelDto.Password);
        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.First().Description);
        await _userManager.AddToRoleAsync(user, AppRoles.Teacher);
    }

    public async Task RegisterAdmin(RegisterModelDto modelDto)
    {
        var userExists = await _userManager.FindByNameAsync(modelDto.Username);
        if (userExists != null)
            throw new BadRequestException(StringConstants.UserAlreadyExists);
        ApplicationUser user = new()
        {
            Email = modelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = modelDto.Username
        };
        var result = await _userManager.CreateAsync(user, modelDto.Password);
        if (!result.Succeeded)
            throw new HttpServerErrorException(HttpStatusCode.InternalServerError, result.Errors.First().Description);
        await _userManager.AddToRoleAsync(user, AppRoles.Admin);
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