using Library.Api.Middleware;
using Library.Application.Configurations;
using Library.Infrastructure.Configurations;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiApplication(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
// Add Controllers  
builder.Services.AddControllers();
//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
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

app.UseCors("ClientPolicy");

// Global error handler
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();
// Output cache  
app.UseOutputCache();
app.MapGet("_health", () => Results.Ok("Ok")).ShortCircuit();

app.Run();