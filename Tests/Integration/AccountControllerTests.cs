using API.DTOs;
using Application.DTOs;
using Newtonsoft.Json;

namespace Tests.Integration
{
    public class AccountControllerTests : BaseControllerTests
    {
        [Fact]
        public async Task MustGetAAccount()
        {
            var accountId = await SignUp("65542538070");
            var accountResponse = await _client.GetAsync($"/api/account?accountId={accountId}");

            var accountResponseContent = await accountResponse.Content.ReadAsStringAsync();
            var accountDto = JsonConvert.DeserializeObject<AccountDto>(accountResponseContent);

            Assert.NotNull(accountDto);
        }

        [Fact]
        public async Task ShoultNotGetAAccountWithInvalidId()
        {
            var accountResponse = await _client.GetAsync($"/api/account?accountId={Guid.Empty}");

            Assert.NotNull(accountResponse);
            Assert.Equal(422, (int)accountResponse.StatusCode);

            var responseContent = await accountResponse.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"Account {Guid.Empty} not found", errorResponseDto.ErrorMessages);
        }
    }
}
