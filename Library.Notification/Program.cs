using Library.Infrastructure.Configurations;
using Library.Notification.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding(o =>
{
    o.JsonSettings.WriteIndented = true;
});

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();

builder.Services.AddMongoDb(builder.Configuration);
// Register Notification Service
var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGrpcService<NotificationServiceImpl>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();