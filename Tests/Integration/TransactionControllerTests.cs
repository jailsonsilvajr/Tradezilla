using API.DTOs;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;
using System.Text;

namespace Tests.Integration
{
    public class TransactionControllerTests : BaseControllerTests
    {
        [Fact]
        public async Task MustMakeACreditTransaction()
        {
            var accountId = await SignUp("61368060021");

            var transactionRequestUri = "/api/transactions/placeCredit";
            var transactionJson = JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                AssetName = "BTC",
                Quantity = 10,
                TransactionType = (int)TransactionType.Credit
            });
            var transactionContent = new StringContent(transactionJson, Encoding.UTF8, "application/json");
            await _client.PostAsync(transactionRequestUri, transactionContent);

            var accountRequestUri = $"/api/accounts/getAccount?accountId={accountId}";
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
        public async Task ShouldNotCreateAnCreditTransactionWithAnInvalidData()
        {
            var requestUri = "/api/transactions/placeCredit";
            var json = JsonConvert.SerializeObject(new { });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(requestUri, content);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotCreateAnCreditTransactionWithAnNullAccountId()
        {
            var json = JsonConvert.SerializeObject(new
            {
                AssetName = "BTC",
                Quantity = 10,
                TransactionType = (int)TransactionType.Credit
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/transactions/placeCredit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"Account {Guid.Empty} not found", errorResponseDto.ErrorMessages);
        }

        [Fact]
        public async Task ShouldNotCreateAnCreditTransactionWithAnEmptyAccountId()
        {
            var json = JsonConvert.SerializeObject(new
            {
                AccountId = Guid.Empty,
                AssetName = "BTC",
                Quantity = 10,
                TransactionType = (int)TransactionType.Credit
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/transactions/placeCredit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"Account {Guid.Empty} not found", errorResponseDto.ErrorMessages);
        }

        [Fact]
        public async Task ShouldNotCreateAnCreditTransactionWithAnInvalidValue()
        {
            var accountId = await SignUp("91921373008");

            var json = JsonConvert.SerializeObject(new
            {
                AccountId = accountId,
                AssetName = "BTC",
                Quantity = 0,
                TransactionType = (int)TransactionType.Credit
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/transactions/placeCredit", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains("Quantity must be greater than 0", errorResponseDto.ErrorMessages);
        }
    }
}
