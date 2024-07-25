using System.IdentityModel.Tokens.Jwt;
using Library.Domain.Auth;

namespace Library.Interfaces.Services;

public interface IAuthService
{
    Task<JwtSecurityToken> Login(LoginModel loginModel);
    Task RegisterTeacher(RegisterModel model);
    Task RegisterAdmin(RegisterModel model);
}