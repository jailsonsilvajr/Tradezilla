using Application.DTOs;
using Domain.Enums;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Tests.Integration
{
    public class DepthControllerTests : BaseControllerTests
    {
        [Fact]
        public async Task GetDepth_WithPrecision0()
        {
            // Arrange
            var accountId = await SignUp("61368060021");
            await MakeTransaction(accountId, "BTC", 10, (int)TransactionType.Credit);
            await MakeTransaction(accountId, "USD", 10, (int)TransactionType.Credit);

            var placeOrderDto = new PlaceOrderDto{ AccountId = accountId, MarketId = "BTC/USD", Side = "Sell", Quantity = 1, Price = 94550m };
            var content = new StringContent(JsonConvert.SerializeObject(placeOrderDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/orders/placeOrder", content);
            
            placeOrderDto.MarketId = "BTC/USD";
            placeOrderDto.Quantity = 2;
            placeOrderDto.Price = 94500;
            content = new StringContent(JsonConvert.SerializeObject(placeOrderDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/orders/placeOrder", content);

            placeOrderDto.MarketId = "BTC/USD";
            placeOrderDto.Quantity = 2;
            placeOrderDto.Price = 94300;
            placeOrderDto.Side = "Buy";
            content = new StringContent(JsonConvert.SerializeObject(placeOrderDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/orders/placeOrder", content);

            // Action
            var getDepthOutput = await _client.GetAsync($"/api/depths/getDepth?marketId=BTC/USD&precision=0");

            // Assert
            getDepthOutput.Should().NotBeNull();
            getDepthOutput.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await getDepthOutput.Content.ReadAsStringAsync();
            var depthDto = JsonConvert.DeserializeObject<DepthDto>(responseContent);

            depthDto.Should().NotBeNull();

            depthDto.Buys.Should().NotBeNull();
            depthDto.Sells.Should().NotBeNull();

            depthDto.Buys.Count.Should().Be(1);
            depthDto.Buys[0].Price.Should().Be(94300);
            depthDto.Buys[0].Quantity.Should().Be(2);

            depthDto.Sells.Count.Should().Be(2);
            depthDto.Sells[0].Price.Should().Be(94550);
            depthDto.Sells[0].Quantity.Should().Be(1);
            depthDto.Sells[1].Price.Should().Be(94500);
            depthDto.Sells[1].Quantity.Should().Be(2);
        }

        [Fact]
        public async Task GetDepth_WithPrecision2()
        {
            // Arrange
            var accountId = await SignUp("61368060021");
            await MakeTransaction(accountId, "BTC", 10, (int)TransactionType.Credit);
            await MakeTransaction(accountId, "USD", 10, (int)TransactionType.Credit);

            var placeOrderDto = new PlaceOrderDto { AccountId = accountId, MarketId = "BTC/USD", Side = "Sell", Quantity = 1, Price = 94550m };
            var content = new StringContent(JsonConvert.SerializeObject(placeOrderDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/orders/placeOrder", content);

            placeOrderDto.MarketId = "BTC/USD";
            placeOrderDto.Quantity = 2;
            placeOrderDto.Price = 94500;
            content = new StringContent(JsonConvert.SerializeObject(placeOrderDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/orders/placeOrder", content);

            placeOrderDto.MarketId = "BTC/USD";
            placeOrderDto.Quantity = 2;
            placeOrderDto.Price = 94400;
            placeOrderDto.Side = "Buy";
            content = new StringContent(JsonConvert.SerializeObject(placeOrderDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/orders/placeOrder", content);

            // Action
            var getDepthOutput = await _client.GetAsync($"/api/depths/getDepth?marketId=BTC/USD&precision=2");

            // Assert
            getDepthOutput.Should().NotBeNull();
            getDepthOutput.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await getDepthOutput.Content.ReadAsStringAsync();
            var depthDto = JsonConvert.DeserializeObject<DepthDto>(responseContent);

            depthDto.Should().NotBeNull();

            depthDto.Buys.Should().NotBeNull();
            depthDto.Sells.Should().NotBeNull();

            depthDto.Buys.Count.Should().Be(1);
            depthDto.Buys[0].Price.Should().Be(94400);

            depthDto.Sells.Count.Should().Be(1);
            depthDto.Sells[0].Price.Should().Be(94500);
            depthDto.Sells[0].Quantity.Should().Be(3);
        }
    }
}
