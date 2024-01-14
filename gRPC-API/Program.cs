using gRPC_API.Services;
using EasyMailCoreApplication.Logic;
using EasyMailCoreApplication.Repositories;
using EasyMailCoreApplication.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

builder.Services.Configure<ValidationConfiguration>(builder.Configuration.GetSection("Validation"));
builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("Database"));

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddSingleton<IEmailLogic, EmailLogic>();
builder.Services.AddSingleton<IEmailRepository, EmailRepository_MSSQL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<EmailAPI>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

IWebHostEnvironment env = app.Environment;

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();