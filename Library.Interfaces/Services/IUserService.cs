using Library.Domain.DTOs;

namespace Library.Interfaces.Services;

public interface IUserService
{
    Task AddUserAsync(UserDto? user);
}