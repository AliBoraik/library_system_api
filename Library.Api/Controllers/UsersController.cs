using Library.Domain.DTOs;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public UsersController(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return Ok(await _userRepository.GetAllUsersAsync());
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null) return NotFound();

        return user;
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.UserId) return BadRequest();

        await _userRepository.UpdateUserAsync(user);

        return NoContent();
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(UserDto user)
    {
        await _userService.AddUserAsync(user);
        return CreatedAtAction("GetUser", new { id = user.UserId }, user);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteUserAsync(id);

        return NoContent();
    }
}