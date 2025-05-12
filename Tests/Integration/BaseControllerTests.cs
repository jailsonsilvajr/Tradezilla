using Newtonsoft.Json;
using System.Text;

namespace Tests.Integration
{
    public class BaseControllerTests
    {
        protected readonly HttpClient _client;

        public BaseControllerTests(HttpClient client)
        {
            _client = client;
        }

        protected async Task<Guid> SignUp(string document)
        {
            var signupRequestUri = "/api/signup";
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
    }
}
