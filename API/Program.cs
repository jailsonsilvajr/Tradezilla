using Application;
using Application.Handlers;
using Application.Notifications;
using Application.Ports.Driven;
using Application.Ports.Driven.Mediator;
using Application.Ports.Driving;
using Application.UseCases;
using DatabaseContext;
using DatabaseContext.Repositories;
using EventSource;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
if (!env.IsEnvironment("Testing"))
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
    builder.Services.AddDbContext<TradezillaContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddTransient<INotificationHandler<PlaceOrderNotification>, PlaceOrderHandler>();
builder.Services.AddTransient<INotificationHandler<PlaceTradeNotification>, PlaceTradeHandler>();
builder.Services.AddSingleton<IMediator, Mediator>();
builder.Services.AddSingleton<IBook, Book>();
builder.Services.AddScoped<ISignUp, SignUpUseCase>();
builder.Services.AddScoped<IPlaceTransaction, PlaceTransactionUseCase>();
builder.Services.AddScoped<IGetUser, GetUserUseCase>();
builder.Services.AddScoped<IPlaceOrder, PlaceOrderUseCase>();
builder.Services.AddScoped<IGetDepth, GetDepthUseCase>();
builder.Services.AddScoped<IGetWallet, GetWalletUseCase>();
builder.Services.AddScoped<IExecuteOrder, ExecuteOrderUseCase>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
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