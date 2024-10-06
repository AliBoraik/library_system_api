using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Library.Api.Controllers;
// {"username": "admin","email": "admin@gmail.com","password": "Adminadmin@123"}
// {"username": "teacher","email": "teacher@example.com","password": "Adminadmin@123"}
// {"username": "student","email": "student@example.com","password": "Adminadmin@123"}

[Route("Api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IOutputCacheStore cacheStore) : ControllerBase
{
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<AuthDataResponse>> Login([FromBody] LoginModelDto modelDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.Login(modelDto);
        return result.Match<ActionResult<AuthDataResponse>>(
            authData => Ok(authData),
            error => StatusCode(error.Code, error));
    }

    [HttpPost]
    [Route("Register-Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModelDto modelDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.RegisterAdmin(modelDto);
        return result.Match<IActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }

    [HttpPost]
    [Route("Register-Teacher")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterTeacher([FromBody] RegisterModelDto modelDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.RegisterTeacher(modelDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Teachers, CancellationToken.None);
        return Ok();
    }

    [HttpPost]
    [Route("Register-Student")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterModelDto modelDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await authService.RegisterStudent(modelDto);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        await cacheStore.EvictByTagAsync(OutputCacheTags.Students, CancellationToken.None);
        return Ok();
    }

    [HttpGet]
    [Route("Validate-Token")]
    [Authorize]
    public IActionResult ValidateToken()
    {
        var authClaims = new Dictionary<string, string>
        {
            { User.FindFirst(AppClaimTypes.Role)!.Type, User.FindFirst(AppClaimTypes.Role)!.Value},
            { User.FindFirst(AppClaimTypes.Id)!.Type, User.FindFirst(AppClaimTypes.Id)!.Value}
        };
        return Ok(authClaims);
    }
}