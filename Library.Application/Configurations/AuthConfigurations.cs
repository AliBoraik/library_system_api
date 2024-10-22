using System.Text;
using Library.Domain.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Library.Application.Configurations;

public static class AuthConfigurations
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection("JwtOptions")
            .Get<JwtOptions>()!;
        services.AddSingleton(jwtOptions);
        // Adding Auth
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Bearer
            .AddJwtBearer(options =>
            {
                //convert the string signing key to byte array
                var signingKeyBytes = Encoding.UTF8
                    .GetBytes(jwtOptions.SigningKey);
                
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    LifetimeValidator = CustomLifetimeValidator
                };
            });

        services.AddAuthorization();
    }

    private static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate,
        TokenValidationParameters param)
    {
        return expires != null && expires > DateTime.UtcNow;
    }
}