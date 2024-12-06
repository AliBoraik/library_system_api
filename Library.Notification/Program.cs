using Library.Application.Configurations;
using Library.Notification.Configurations;
using Library.Notification.Consumers;
using Library.Notification.Middleware;

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

// Register Notification Service
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

// Global error handler
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
// Output cache  
app.UseOutputCache();
app.MapGet("_health", () => Results.Ok("Ok")).ShortCircuit();
app.Run();