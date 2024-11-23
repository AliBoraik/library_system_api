using Library.Api.Middleware;
using Library.Application.Configurations;
using Library.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAppl(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
// Redis OutputCache
builder.Services.RedisOutputCache(builder.Configuration);
//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .WithExposedHeaders("Content-Disposition")
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ClientPermission");

// Global error handler
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseOutputCache();
app.MapGet("_health", () => Results.Ok()).ShortCircuit();

app.Run();