using BuggyBuddyAPI.Contracts;
using BuggyBuddyAPI.Models;
using BuggyBuddyAPI.Repositorys;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserContract, UserRepository>();
builder.Services.AddScoped<IBuggyContract, BuggyRepository>();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DataBase"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
