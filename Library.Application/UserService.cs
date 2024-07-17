using AutoMapper;
using Library.Domain.DTOs;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Application;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task AddUserAsync(UserDto? userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userRepository.AddUserAsync(user);
    }
}