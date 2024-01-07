using pkaselj_lab_07_.Configuration;
using pkaselj_lab_07_.Logic;
using pkaselj_lab_07_.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IEmailLogic, EmailLogic>();
builder.Services.AddSingleton<IEmailRepository, EmailRepository_SQL>();

builder.Services.Configure<ValidationConfiguration>(builder.Configuration.GetSection("Validation"));
builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("Database"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
