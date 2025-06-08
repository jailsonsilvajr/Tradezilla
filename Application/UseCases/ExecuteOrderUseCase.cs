using Application.Ports.Driving;

namespace Application.UseCases
{
    public class ExecuteOrderUseCase : IExecuteOrder
    {
        private readonly IBook _book;

        public ExecuteOrderUseCase(IBook book)
        {
            _book = book;
        }

        public async Task ExecuteOrderAsync(string marketId)
        {
            await _book.MatchOrderAsync(marketId);
        }
    }
}
