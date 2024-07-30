using Library.Api.Middleware;
using Library.Application.Configurations;
using Library.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handler
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseOutputCache();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("_health", () => Results.Ok()).ShortCircuit();

app.Run();