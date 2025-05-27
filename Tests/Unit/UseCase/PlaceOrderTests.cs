using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driven.Mediator;
using Application.UseCases;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Moq;

namespace Tests.Unit.UseCase
{
    public class PlaceOrderTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly PlaceOrderUseCase _placeOrderUseCase;
        private readonly Mock<IMediator> _mediatorMock;

        public PlaceOrderTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();

            _placeOrderUseCase = new PlaceOrderUseCase(
                _accountRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mediatorMock.Object);
        }

        [Fact]
        public async Task AccountNotFound_ThrowsEntityNotFoundException()
        {
            var placeOrderDto = new PlaceOrderDto
            {
                AccountId = Guid.NewGuid(),
                MarketId = "BTC/USD",
                Side = "Buy",
                Quantity = 1,
                Price = 1000.00m
            };

            _accountRepositoryMock.Setup(repo => repo.GetAccountByAccountIdAsync(placeOrderDto.AccountId))
                .ReturnsAsync((Account?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _placeOrderUseCase.PlaceOrder(placeOrderDto));
        }

        [Fact]
        public async Task MustPlaceOrder()
        {
            var placeOrderDto = new PlaceOrderDto
            {
                AccountId = Guid.NewGuid(),
                MarketId = "BTC/USD",
                Side = "Buy",
                Quantity = 1,
                Price = 1000.00m
            };

            var account = Account.Create(
                    name: "John Doe",
                    email: "johndoe@gmail.com",
                    document: "58865866012",
                    password: "asdQWE123");

            var asset = Asset.Create(account.AccountId, "USD");
            var transaction = Transaction.Create(asset.AssetId, placeOrderDto.Price, TransactionType.Credit);
            asset.AddTransaction(transaction);
            account.AddAsset(asset);
            _accountRepositoryMock.Setup(repo => repo.GetAccountByAccountIdAsync(placeOrderDto.AccountId)).ReturnsAsync(account);

            var orderId = await _placeOrderUseCase.PlaceOrder(placeOrderDto);

            Assert.NotEqual(Guid.Empty, orderId);
        }
    }
}
