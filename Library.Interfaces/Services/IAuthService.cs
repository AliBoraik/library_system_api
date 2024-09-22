using Library.Domain;
using Library.Domain.Auth;

namespace Library.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthDataResponse, Error>> Login(LoginModelDto loginModelDto);
    Task<Result<Ok, Error>> RegisterTeacher(RegisterModelDto modelDto);
    Task<Result<Ok, Error>> RegisterStudent(RegisterModelDto modelDto);
    Task<Result<Ok, Error>> RegisterAdmin(RegisterModelDto modelDto);
}