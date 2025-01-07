using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Library.Domain.Auth;
using Library.Domain.Constants;
using Library.Domain.Events.Notification;
using Library.Domain.Models;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Auth.Services;

public class AuthService(UserManager<User> userManager, ITokenService tokenService,IProducerService producerService, AppDbContext context)
    : IAuthService
{
    public async Task<Result<AuthDataResponse, Error>> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return Result<AuthDataResponse, Error>.Err(Errors.NotFound("user"));
        if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
            return Result<AuthDataResponse, Error>.Err(Errors.BadRequest("Incorrect password"));

        var userRoles = await userManager.GetRolesAsync(user);
        var accessAuthClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        accessAuthClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var refreshAuthClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var generatedAccessToken = tokenService.CreateAccessToken(accessAuthClaims);
        var refreshToken = tokenService.CreateRefreshToken(refreshAuthClaims);

        return Result<AuthDataResponse, Error>.Ok(new AuthDataResponse
        {
            AccessToken = generatedAccessToken.AccessToken,
            RefreshToken = refreshToken,
            ExpirationTime = generatedAccessToken.ValidTo
        });
    }

    public async Task<Result<AuthDataResponse, Error>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var tokenValidationResult = await tokenService.RefreshTokenValidationResult(refreshTokenDto.RefreshToken);

        if (!tokenValidationResult.IsValid)
            return Result<AuthDataResponse, Error>.Err(Errors.Unauthorized("refresh token"));

        var accessTokenValidationResult = await tokenService.AccessTokenValidationResult(refreshTokenDto.AccessToken);

        if (!accessTokenValidationResult.IsValid)
            return Result<AuthDataResponse, Error>.Err(Errors.Unauthorized("refresh token"));

        var accessAuthClaims = accessTokenValidationResult.ClaimsIdentity.Claims.ToList();

        var generatedAccessToken = tokenService.CreateAccessToken(accessAuthClaims);

        return Result<AuthDataResponse, Error>.Ok(new AuthDataResponse
        {
            AccessToken = generatedAccessToken.AccessToken,
            RefreshToken = refreshTokenDto.RefreshToken,
            ExpirationTime = generatedAccessToken.ValidTo
        });
    }

    public async Task<Result<Guid, Error>> RegisterTeacher(RegisterTeacherDto dto)
    {
        var userExists = await userManager.FindByEmailAsync(dto.Email);
        if (userExists != null)
            return Result<Guid, Error>.Err(Errors.Conflict("user"));

        var department = await context.Departments.AnyAsync(d => d.Id == dto.DepartmentId);

        if (!department)
            return Result<Guid, Error>.Err(Errors.NotFound("department"));

        var user = new User
        {
            Email = dto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = dto.Username,
            DepartmentId = dto.DepartmentId
        };

        var createUserResult = await userManager.CreateAsync(user, dto.Password);
        if (!createUserResult.Succeeded)
            return Result<Guid, Error>.Err(Errors.InternalServerError());
        // Create Teacher
        var teacher = new Teacher
        {
            Id = user.Id
        };
        await context.Teachers.AddAsync(teacher);
        await userManager.AddToRoleAsync(user, AppRoles.Teacher);

        return Result<Guid, Error>.Ok(user.Id);
    }

    public async Task<Result<Guid, Error>> RegisterStudent(RegisterStudentDto dto)
    {
        var userExists = await userManager.FindByEmailAsync(dto.Email);
        if (userExists != null)
            return Result<Guid, Error>.Err(Errors.Conflict("user"));

        var department = await context.Departments.AnyAsync(d => d.Id == dto.DepartmentId);

        if (!department)
            return Result<Guid, Error>.Err(Errors.NotFound("department"));

        var user = new User
        {
            Email = dto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = dto.Username,
            DepartmentId = dto.DepartmentId
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return Result<Guid, Error>.Err(Errors.InternalServerError());
        // Create Student
        var student = new Student
        {
            Id = user.Id
        };
        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();
        await userManager.AddToRoleAsync(user, AppRoles.Student);
        
        // Run sending notification in the background
        _ = Task.Run(async () =>
        {
            // Create the notification event
            var notificationEvent = new NotificationEvent
            {
                Title = "Welcome to the System!",
                Message = "Hello and welcome to our system! We're excited to have you on board. If you need any help, feel free to reach out to support.",
                SenderUserId = AdminConstants.SystemAdminId, 
                RecipientUserId = user.Id
            };
            // Send notification in the background
            await producerService.SendNotificationEventAsync(AppTopics.NotificationTopic, notificationEvent);
        });
        return Result<Guid, Error>.Ok(user.Id);
    }

    public async Task<Result<Guid, Error>> RegisterAdmin(RegisterAdminDto adminDto)
    {
        var userExists = await userManager.FindByEmailAsync(adminDto.Email);
        if (userExists != null)
            return Result<Guid, Error>.Err(Errors.Conflict("user"));
        var user = new User
        {
            Email = adminDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = adminDto.Username
        };
        var result = await userManager.CreateAsync(user, adminDto.Password);
        if (!result.Succeeded)
            return Result<Guid, Error>.Err(Errors.InternalServerError());
        await userManager.AddToRoleAsync(user, AppRoles.Admin);
        return Result<Guid, Error>.Ok(user.Id);
    }
}