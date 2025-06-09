using API.DTOs;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Tests.Integration
{
    public class OrderControllerTests : BaseControllerTests
    {
        [Fact]
        public async Task PlaceOrder_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var accountId = await SignUp("61368060021");
            await MakeTransaction(accountId, "BTC", 1, (int)TransactionType.Credit);

            var placeOrderDto = new PlaceOrderDto
            {
                AccountId = accountId,
                MarketId = "BTC/USD",
                Side = "Sell",
                Quantity = 1,
                Price = 1000m
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(placeOrderDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders/placeOrder", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var orderId = JsonConvert.DeserializeObject<Guid>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            orderId.Should().NotBe(Guid.Empty);

            var savedOrder = await _context.Orders.FindAsync(orderId);
            savedOrder.Should().NotBeNull();
            savedOrder!.AccountId.Should().Be(placeOrderDto.AccountId);
            savedOrder.Market.Should().Be(placeOrderDto.MarketId);
            savedOrder.Side.Should().Be(placeOrderDto.Side);
            savedOrder.Quantity.Should().Be(placeOrderDto.Quantity);
            savedOrder.Price.Should().Be(placeOrderDto.Price);
            savedOrder.Status.Should().Be("open");
        }

        [Fact]
        public async Task PlaceOrder_WithNonexistentAccount_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            var placeOrderDto = new PlaceOrderDto
            {
                AccountId = Guid.NewGuid(),
                MarketId = "BTC/USD",
                Side = "Buy",
                Quantity = 1,
                Price = 1000m
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(placeOrderDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders/placeOrder", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            errorResponse.Should().NotBeNull();
            errorResponse!.ErrorMessages.Should().ContainSingle()
                .Which.Should().Be($"Account {placeOrderDto.AccountId} not found");
        }

        [Fact]
        public async Task PlaceOrder_WithInsufficientBalance_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            var accountId = await SignUp("74500587071");
            await MakeTransaction(accountId, "USD", 1, (int)TransactionType.Credit);

            var placeOrderDto = new PlaceOrderDto
            {
                AccountId = accountId,
                MarketId = "BTC/USD",
                Side = "Buy",
                Quantity = 10,
                Price = 1000m
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(placeOrderDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders/placeOrder", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDto>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            errorResponse.Should().NotBeNull();
            errorResponse!.ErrorMessages.Should().ContainSingle()
                .Which.Should().Be("Insufficient balance for asset USD");
        }

        [Fact]
        public async Task MustExecuteOrder()
        {
            var accountIdSell = await SignUp("93618553013");
            await MakeTransaction(accountIdSell, "BTC", 1, (int)TransactionType.Credit);
            var placeOrderSellDto = new PlaceOrderDto
            {
                AccountId = accountIdSell,
                MarketId = "BTC/USD",
                Side = "Sell",
                Quantity = 1,
                Price = 94000m
            };

            var contentSell = new StringContent(
                JsonConvert.SerializeObject(placeOrderSellDto),
                Encoding.UTF8,
                "application/json");

            await _client.PostAsync("/api/orders/placeOrder", contentSell);

            var accountIdBuy = await SignUp("74500587071");
            await MakeTransaction(accountIdBuy, "USD", 1, (int)TransactionType.Credit);
            var placeOrderBuyDto = new PlaceOrderDto
            {
                AccountId = accountIdBuy,
                MarketId = "BTC/USD",
                Side = "Buy",
                Quantity = 1,
                Price = 94500m
            };

            var contentBuy = new StringContent(
                JsonConvert.SerializeObject(placeOrderBuyDto),
                Encoding.UTF8,
                "application/json");

            await _client.PostAsync("/api/orders/placeOrder", contentBuy);
            var getDepthOutput = await _client.GetAsync($"/api/depths/getDepth?marketId=BTC/USD&precision=0");

            getDepthOutput.Should().NotBeNull();
            getDepthOutput.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await getDepthOutput.Content.ReadAsStringAsync();
            var depthDto = JsonConvert.DeserializeObject<DepthDto>(responseContent);

            depthDto.Should().NotBeNull();

            depthDto.Buys.Should().NotBeNull();
            depthDto.Sells.Should().NotBeNull();

            depthDto.Buys.Count.Should().Be(0);
            depthDto.Sells.Count.Should().Be(0);
        }
    }
} 