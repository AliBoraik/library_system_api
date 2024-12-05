using Library.Application.Configurations;
using Library.Notification.Configurations;
using Library.Notification.Consumers;
using Library.Notification.Middleware;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// add auth service Collections
builder.Services.AddNotificationApplication(builder.Configuration);
// Add Consumer Configuration
builder.Services.AddConsumerConfig(builder.Configuration);
// Add Swagger Configuration
builder.Services.AddSwaggerConfiguration();
// add background services 
builder.Services.AddHostedService<NotificationSubscriberBackground>();
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

// Register Notification Service
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Output cache  
app.UseOutputCache();
app.MapGet("_health", () => Results.Ok("Ok")).ShortCircuit();
app.Run();