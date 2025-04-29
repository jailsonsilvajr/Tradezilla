using System.Net;

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

            var response = await _client.PostAsync(requestUri, null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
