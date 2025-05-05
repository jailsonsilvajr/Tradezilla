using Application.Ports.Driven;
using Application.Ports.Driving;
using Application.UseCases;
using DatabaseContext;
using DatabaseContext.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
if (!env.IsEnvironment("Testing"))
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
    builder.Services.AddDbContext<TradezillaContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddScoped<ISignUp, SignUpUseCase>();
builder.Services.AddScoped<IDeposit, DepositUseCase>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDepositRepository, DepositRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

await app.RunAsync();
public partial class Program { }