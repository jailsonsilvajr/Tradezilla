using DatabaseContext;
using Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace Tests.Integration
{
    public class BaseControllerTests : IDisposable
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly HttpClient _client;
        protected readonly TradezillaContext _context;
        private SqliteConnection? _connection;

        public BaseControllerTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");
                    builder.ConfigureServices(services =>
                    {
                        _connection = new SqliteConnection("DataSource=:memory:");
                        _connection.Open();

                        services.AddDbContext<TradezillaContext>(options =>
                            options.UseSqlite(_connection));
                    });
                });

            _client = _factory.CreateClient();
            _context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<TradezillaContext>();
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _client.Dispose();
            _factory.Dispose();
        }

        protected async Task<Guid> SignUp(string document)
        {
            var signupRequestUri = "/api/accounts/signUp";
            var signupJson = JsonConvert.SerializeObject(new
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Document = document,
                Password = "asdQWE123"
            });
            var signupContent = new StringContent(signupJson, Encoding.UTF8, "application/json");
            var signupResponse = await _client.PostAsync(signupRequestUri, signupContent);

            var signupResponseContent = await signupResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Guid>(signupResponseContent)!;
        }

        protected async Task MakeTransaction(Guid accountId, string assetName, int value, int transactionType)
        {
            var transactionRequestUri = transactionType == (int)TransactionType.Credit
                ? "/api/transactions/placeCredit"
                : "/api/transactions/placeDebit";

            var transactionJson = JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                AssetName = assetName,
                Quantity = value,
                TransactionType = transactionType
            });

            var transactionContent = new StringContent(transactionJson, Encoding.UTF8, "application/json");
            await _client.PostAsync(transactionRequestUri, transactionContent);
        }
    }
}
