using Library.Domain;
using Library.Domain.Auth;

namespace Library.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthDataResponse, Error>> LoginAsync(LoginDto loginDto);
    Task<Result<AuthDataResponse, Error>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    Task<Result<Guid, Error>> RegisterTeacher(RegisterTeacherDto dto);
    Task<Result<Guid, Error>> RegisterStudent(RegisterStudentDto dto);
    Task<Result<Guid, Error>> RegisterAdmin(RegisterDto dto);
}