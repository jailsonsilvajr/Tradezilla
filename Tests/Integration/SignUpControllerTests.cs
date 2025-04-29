using API.DTOs;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Tests.Integration
{
    public class SignUpControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SignUpControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task MustCreateAValidAccount()
        {
            var requestUri = "/api/signup";
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
            var accountDto = JsonConvert.DeserializeObject<AccountDto>(responseContent);

            Assert.NotNull(accountDto);
            Assert.NotEqual(default, accountDto.Id);
            Assert.Equal("John Doe", accountDto.Name);
            Assert.Equal("john.doe@gmail.com", accountDto.Email);
            Assert.Equal("97456321558", accountDto.Document);
            Assert.Equal("asdQWE123", accountDto.Password);
        }

        [Fact]
        public async Task SignUp_ReturnsBadRequest_WhenInvalidData()
        {
            var requestUri = "/api/signup";
            var json = JsonConvert.SerializeObject(new {});
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(requestUri, content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotCreateAnAccountWithAnInvalidName()
        {
            var json = JsonConvert.SerializeObject(new
            {
                Name = "John",
                Email = "john.doe@gmail.com",
                Document = "97456321558",
                Password = "asdQWE123"
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/signup", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains("Invalid name", errorResponseDto.ErrorMessages);
        }

        [Fact]
        public async Task ShouldNotCreateAnAccountWithAnNullName()
        {
            var json = JsonConvert.SerializeObject(new
            {
                Email = "john.doe@gmail.com",
                Document = "97456321558",
                Password = "asdQWE123"
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/signup", content);

            Assert.NotNull(response);
            Assert.Equal(422, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponseDto = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            Assert.NotNull(errorResponseDto);
            Assert.Contains("Name can't be null", errorResponseDto.ErrorMessages);
        }
    }
}
