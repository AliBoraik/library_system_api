using System.IdentityModel.Tokens.Jwt;
using Library.Domain;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;
// {"username": "admin","email": "admin@gmail.com","password": "Adminadmin@123"}
// {"username": "teacher","email": "teacher@example.com","password": "Teacherteacher@123"}

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticateController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var token = await _authService.Login(model);
        return Ok(new AuthDataResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });
    }

    [HttpPost]
    [Route("register-teacher")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterTeacher([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _authService.RegisterTeacher(model);
        return Ok(new Response { StatusText = ResponseStatus.Success, Data = StringConstants.UserCreatedSuccessfully });
    }

    [HttpPost]
    [Route("register-admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _authService.RegisterAdmin(model);
        return Ok(new Response { StatusText = ResponseStatus.Success, Data = StringConstants.UserCreatedSuccessfully });
    }
}