using Library.Application.Configurations;
using Library.Auth.Configurations;
using Library.Auth.Middleware;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);


// add auth service Collections
builder.Services.AddAuthApplication(builder.Configuration);
// Add Swagger Configuration
builder.Services.AddSwaggerConfiguration();
// Add services to the container.
builder.Services.AddControllers();
// add cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .WithExposedHeaders("Content-Disposition")
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

// Global error handler
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();
// Output cache  
app.UseOutputCache();
app.MapGet("_health", () => Results.Ok("Ok")).ShortCircuit();
app.Run();