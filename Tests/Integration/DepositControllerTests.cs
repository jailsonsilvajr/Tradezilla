using API.DTOs;
using Application.DTOs;
using Domain.Entities;
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

        [Fact]
        public async Task ShouldNotCreateAnDepositWithAnInvalidData()
        {
            var requestUri = "/api/deposit";
            var json = JsonConvert.SerializeObject(new { });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(requestUri, content);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotCreateAnDepositWithAnNullAccountId()
        {
            var json = JsonConvert.SerializeObject(new
            {
                AssertId = "BTC",
                Quantity = 10
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/deposit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains("AccountId cannot be empty", errorResponseDto.ErrorMessages);
        }

        [Fact]
        public async Task ShouldNotCreateAnDepositWithAnEmptyAccountId()
        {
            var json = JsonConvert.SerializeObject(new
            {
                AccountId = Guid.Empty,
                AssertId = "BTC",
                Quantity = 10
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/deposit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains("AccountId cannot be empty", errorResponseDto.ErrorMessages);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("BTCBTC")]
        public async Task ShouldNotCreateAnDepositWithAnInvalidAssetId(string? assetId)
        {
            var json = JsonConvert.SerializeObject(new
            {
                AccountId = Guid.NewGuid(),
                AssertId = assetId,
                Quantity = 10
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/deposit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"AssetId must be between 1 and {Deposit.MAX_ASSETID_LENGTH} characters long", errorResponseDto.ErrorMessages);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task ShouldNotCreateAnDepositWithAnInvalidQuantity(int quantity)
        {
            var json = JsonConvert.SerializeObject(new
            {
                AccountId = Guid.Empty,
                AssertId = "BTC",
                Quantity = quantity
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/deposit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains("Quantity must be greater than 0", errorResponseDto.ErrorMessages);
        }
    }
}
