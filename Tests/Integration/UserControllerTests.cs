﻿using API.DTOs;
using Application.DTOs;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Tests.Integration
{
    public class UserControllerTests : BaseControllerTests
    {
        [Fact]
        public async Task MustCreateAValidAccount()
        {
            var requestUri = "/api/users/signUp";
            var json = JsonConvert.SerializeObject(new
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Document = "97456321558",
                Password = "asdQWE123"
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(requestUri, content);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var accountId = JsonConvert.DeserializeObject<Guid>(responseContent);

            Assert.NotEqual(Guid.Empty, accountId);
        }

        [Fact]
        public async Task ShouldNotCreateAnAccountWithAnInvalidData()
        {
            var requestUri = "/api/users/signUp";
            var json = JsonConvert.SerializeObject(new { });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(requestUri, content);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task MustGetAUser()
        {
            var accountId = await SignUp("65542538070");
            var accountResponse = await _client.GetAsync($"/api/users/getUser?accountId={accountId}");

            var accountResponseContent = await accountResponse.Content.ReadAsStringAsync();
            var accountDto = JsonConvert.DeserializeObject<UserDto>(accountResponseContent);

            Assert.NotNull(accountDto);
        }

        [Fact]
        public async Task ShoultNotGetAAccountWithInvalidId()
        {
            var accountResponse = await _client.GetAsync($"/api/users/getUser?accountId={Guid.Empty}");

            Assert.NotNull(accountResponse);
            Assert.Equal(422, (int)accountResponse.StatusCode);

            var responseContent = await accountResponse.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains($"User with AccountId {Guid.Empty} not found", errorResponseDto.ErrorMessages);
        }
    }
}
