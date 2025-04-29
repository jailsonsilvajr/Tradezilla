using API.DTOs;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

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
        public async Task SignUp_ReturnsOk()
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

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
    }
}
