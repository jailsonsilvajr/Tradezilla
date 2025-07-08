using Application;
using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driven.Mediator;
using Application.UseCases;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Moq;

namespace Tests.Unit.UseCase
{
    public class PlaceOrderTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly PlaceOrderUseCase _placeOrderUseCase;
        private readonly Mock<IMediator> _mediatorMock;

        public PlaceOrderTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();

            _placeOrderUseCase = new PlaceOrderUseCase(
                _walletRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mediatorMock.Object,
                new Book(_mediatorMock.Object));
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

            _walletRepositoryMock.Setup(repo => repo.GetWalletByAccountIdAsync(placeOrderDto.AccountId))
                .ThrowsAsync(new EntityNotFoundException("Account not found"));

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

            var accountId = Guid.NewGuid();

            var asset = Asset.Create(accountId, "USD");
            var transaction = Transaction.Create(asset.GetId(), placeOrderDto.Quantity, TransactionType.Credit);
            var wallet = new Wallet(accountId, new() { asset }, new(), new() { transaction });

            _walletRepositoryMock.Setup(repo => repo.GetWalletByAccountIdAsync(placeOrderDto.AccountId)).ReturnsAsync(wallet);

            var orderId = await _placeOrderUseCase.PlaceOrder(placeOrderDto);

            Assert.NotEqual(Guid.Empty, orderId);
        }
    }
}
