using Library.Domain.Constants;
using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController(ApplicationDbContext context) :  ControllerBase
{
    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<IEnumerable<User>>> GetTeachers()
    {
        var teachers =  await context.Users
            .Where(u => u.UserType == UserType.Teacher)
            .Select(t => new { t.Id ,  t.UserName, t.Email })
            .ToListAsync();
        return Ok(teachers);
    }
}