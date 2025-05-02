using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;

namespace Application.UseCases
{
    public class SignUpUseCase : ISignUp
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SignUpUseCase(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> SingUpAsync(SignUpDto signUpDto)
        {
            var account = Account.Create(
                signUpDto.Name,
                signUpDto.Email,
                signUpDto.Document,
                signUpDto.Password);

            _accountRepository.RegisterAccount(account);
            await _unitOfWork.SaveChangesAsync();

            return account.AccountId;
        }
    }
}
