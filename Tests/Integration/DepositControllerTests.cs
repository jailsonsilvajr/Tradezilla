using API.DTOs;
using Application.DTOs;
using Domain.Entities;
using Newtonsoft.Json;
using System.Text;

namespace Tests.Integration
{
    public class DepositControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public DepositControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory.CreateClient())
        {
        }

        [Fact]
        public async Task MustMakeADeposit()
        {
            var accountId = await SignUp(new SignUpDto
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Document = "61368060021",
                Password = "asdQWE123"
            });

            var depositRequestUri = "/api/deposit";
            var depositJson = JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                AssetName = "BTC",
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
            Assert.Single(accountDto.Assets);
            Assert.All(accountDto.Assets, asset => Assert.Equal("BTC", asset.AssetName));
            Assert.All(accountDto.Assets, asset => Assert.Equal(10, asset.Balance));
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
                AssetName = "BTC",
                Quantity = 10
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/deposit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"Account {Guid.Empty} not found", errorResponseDto.ErrorMessages);
        }

        [Fact]
        public async Task ShouldNotCreateAnDepositWithAnEmptyAccountId()
        {
            var json = JsonConvert.SerializeObject(new
            {
                AccountId = Guid.Empty,
                AssetName = "BTC",
                Quantity = 10
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/deposit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"Account {Guid.Empty} not found", errorResponseDto.ErrorMessages);
        }

        [Theory]
        [InlineData(null, "53688039076")]
        [InlineData("", "15142888006")]
        [InlineData("BTCBTC", "16599447082")]
        public async Task ShouldNotCreateAnDepositWithAnInvalidAssetName(string? assetName, string document)
        {
            var accountId = await SignUp(new SignUpDto
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Document = document,
                Password = "asdQWE123"
            });

            var json = JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                AssetName = assetName,
                Quantity = 10
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/deposit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"AssetName must be between 1 and {Asset.MAX_ASSETNAME_LENGTH} characters long", errorResponseDto.ErrorMessages);
        }

        [Theory]
        [InlineData(0, "91921373008")]
        [InlineData(-1, "20493335013")]
        public async Task ShouldNotCreateAnDepositWithAnInvalidQuantity(int quantity, string document)
        {
            var accountId = await SignUp(new SignUpDto
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Document = document,
                Password = "asdQWE123"
            });

            var json = JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                AssetName = "BTC",
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
