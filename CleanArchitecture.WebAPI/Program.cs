using CleanArchitecture.Application.Queries;
using CleanArchitecture.Application.Commands;
using CleanArchitecture.Infrastructure.Queries;
using CleanArchitecture.Infrastructure.Commands;
using CleanArchitecture.Application.Commands.BrokerManager;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationCommandServices(builder.Configuration);
builder.Services.AddApplicationQueryServices(builder.Configuration);

builder.Services.AddInfrastructureCommandServices(builder.Configuration);
builder.Services.AddInfrastructureQueryServices(builder.Configuration);

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
