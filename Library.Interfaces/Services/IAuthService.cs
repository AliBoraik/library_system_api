using Library.Domain.Auth;
using Library.Domain.Results;
using Library.Domain.Results.Common;

namespace Library.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthDataResponse, Error>> LoginAsync(LoginDto loginDto);
    Task<Result<AuthDataResponse, Error>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    Task<Result<Guid, Error>> RegisterTeacher(RegisterTeacherDto dto);
    Task<Result<Guid, Error>> RegisterStudent(RegisterStudentDto dto);
    Task<Result<Guid, Error>> RegisterAdmin(RegisterAdminDto adminDto);
}