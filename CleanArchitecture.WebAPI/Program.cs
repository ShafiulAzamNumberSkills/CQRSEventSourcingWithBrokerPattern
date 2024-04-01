using CleanArchitecture.Application;
using CleanArchitecture.Application.Commands.BrokerManager;
using CleanArchitecture.Domain.Data;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PostContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplicationCommandServices(builder.Configuration);
builder.Services.AddApplicationQueryServices(builder.Configuration);

var provider = builder.Services.BuildServiceProvider();

var brokerManager = provider.GetRequiredService<IBrokerManager>();

Thread brokerManagerThread = new Thread(new ThreadStart(brokerManager.consume));
brokerManagerThread.Start();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
