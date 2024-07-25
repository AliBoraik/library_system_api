using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Domain;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Library.Api.Controllers;
// {"username": "admin","email": "admin@gmail.com","password": "Adminadmin@123"}
// {"username": "teacher","email": "teacher@example.com","password": "Teacherteacher@123"}

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticateController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null) return Unauthorized();
        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized(new Response { StatusText = ResponseStatus.Error, Message = UserMessageConstants.IncorrectPassword });
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        var token = GetToken(authClaims);
        return Ok(new 
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

    [HttpPost]
    [Route("register-teacher")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterTeacher([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { StatusText = ResponseStatus.Error, Message = UserMessageConstants.UserAlreadyExists });

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { StatusText = ResponseStatus.Error, Message = result.Errors.First().Description });

        await _userManager.AddToRoleAsync(user, AppRoles.Teacher);
        return Ok( new Response { StatusText = ResponseStatus.Success, Message = UserMessageConstants.UserCreatedSuccessfully });
    }

    [HttpPost]
    [Route("register-admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { StatusText = ResponseStatus.Error, Message = ResponseMessage.AlreadyExists });

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        foreach (var resultError in result.Errors) Console.WriteLine(resultError.Description);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { StatusText = ResponseStatus.Error, Message = UserMessageConstants.UserCreationFailed });
        await _userManager.AddToRoleAsync(user, AppRoles.Admin);
        return Ok( new Response { StatusText = ResponseStatus.Success, Message = UserMessageConstants.UserCreatedSuccessfully });
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