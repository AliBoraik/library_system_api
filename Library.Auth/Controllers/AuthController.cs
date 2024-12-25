using System.Security.Claims;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Auth.Controllers;
// {"email": "admin@gmail.com","password": "Adminadmin@123"}
// {"email": "teacher@gmail.com","password": "Adminadmin@123"}
// {"email": "student@gmail.com","password": "Adminadmin@123"}

[Route("Api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IOutputCacheStore cacheStore) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and generates a JWT token.
    /// </summary>
    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthDataResponse>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.LoginAsync(loginDto);
        return result.Match<ActionResult<AuthDataResponse>>(
            authData => Ok(authData),
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    /// Registers a new admin user.
    /// </summary>
    [HttpPost]
    [Route("Register-Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.RegisterAdmin(registerDto);
        return result.Match<IActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }
    
    /// <summary>
    /// Registers a new teacher user.
    /// </summary>
    [HttpPost]
    [Route("Register-Teacher")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherDto registerTeacherDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.RegisterTeacher(registerTeacherDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Teachers, CancellationToken.None);
        var id = result.Value;
        return Ok(new { id });
    }

    /// <summary>
    /// Registers a new student user.
    /// </summary>
    [HttpPost]
    [Route("Register-Student")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto registerStudentDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.RegisterStudent(registerStudentDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Students, CancellationToken.None);
        var id = result.Value;
        return Ok(new { id });
    }

    /// <summary>
    /// Refreshes an expired JWT token.
    /// </summary>
    [HttpPost]
    [Route("Refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthDataResponse>> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.RefreshTokenAsync(refreshTokenDto);
        return result.Match<ActionResult<AuthDataResponse>>(
            authData => Ok(authData),
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    /// Validates the provided JWT token.
    /// </summary>
    [HttpGet]
    [Route("Validate-Token")]
    public IActionResult ValidateToken()
    {
        return Ok(new
        {
            Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
            Role = User.FindFirst(ClaimTypes.Role)!.Value
        });
    }
}
