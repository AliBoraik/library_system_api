using Library.Domain.Auth;

namespace Library.Interfaces.Services;

public interface IAuthService
{
    Task<AuthDataResponse> Login(LoginModelDto loginModelDto);
    Task RegisterTeacher(RegisterModelDto modelDto);
    Task RegisterAdmin(RegisterModelDto modelDto);
}