using Library.Domain;
using Library.Domain.Auth;

namespace Library.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthDataResponse, Error>> LoginAsync(LoginDto loginDto);
    Task<Result<AuthDataResponse, Error>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    Task<Result<Ok, Error>> RegisterTeacher(RegisterDto dto);
    Task<Result<Ok, Error>> RegisterStudent(RegisterDto dto);
    Task<Result<Ok, Error>> RegisterAdmin(RegisterDto dto);
}