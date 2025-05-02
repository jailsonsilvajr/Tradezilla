using Application.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace Tests.Integration
{
    public class DepositControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DepositControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task MustMakeADeposit()
        {
            var signupRequestUri = "/api/signup";
            var signupJson = JsonConvert.SerializeObject(new
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Document = "97456321558",
                Password = "asdQWE123"
            });
            var signupContent = new StringContent(signupJson, Encoding.UTF8, "application/json");
            var signupResponse = await _client.PostAsync(signupRequestUri, signupContent);

            var signupResponseContent = await signupResponse.Content.ReadAsStringAsync();
            var accountId = JsonConvert.DeserializeObject<Guid>(signupResponseContent)!;

            var depositRequestUri = "/api/deposit";
            var depositJson = JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                AssetId = "BTC",
                Quantity = 10
            });
            var depositContent = new StringContent(depositJson, Encoding.UTF8, "application/json");
            await _client.PostAsync(depositRequestUri, depositContent);

            var accountRequestUri = $"/api/account?accountId={accountId}";
            var accountResponse = await _client.GetAsync(accountRequestUri);

            var accountResponseContent = await accountResponse.Content.ReadAsStringAsync();
            var accountDto = JsonConvert.DeserializeObject<AccountDto>(accountResponseContent);

            Assert.NotNull(accountDto);
            Assert.NotNull(accountDto.Assets);
            Assert.Collection(accountDto.Assets,
                asset => Assert.Equal("BTC", asset.AssetId),
                asset => Assert.Equal(10, asset.Quantity));
        }
    }
}
