using Application.Ports.Driven;
using Application.Ports.Driving;

namespace Application.UseCases
{
    public class DeleteAccountUseCase : IDeleteAccountUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAccountUseCase(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteAccountByIdAsync(Guid accountId)
        {
            await _accountRepository.DeleteAccountByIdAsync(accountId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
