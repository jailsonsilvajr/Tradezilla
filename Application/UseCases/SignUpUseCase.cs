using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Aggregates;

namespace Application.UseCases
{
    public class SignUpUseCase : ISignUp
    {
        private readonly IUserRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SignUpUseCase(IUserRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> SingUpAsync(SignUpDto signUpDto)
        {
            var user = await _accountRepository.GetUserByDocumentAsync(signUpDto.Document);
            if (user is not null)
            {
                throw new ArgumentException($"User with document {signUpDto.Document} already exists");
            }

            user = User.Create(
                signUpDto.Name,
                signUpDto.Email,
                signUpDto.Document,
                signUpDto.Password);

            _accountRepository.RegisterAccount(user);
            await _unitOfWork.SaveChangesAsync();

            return user.GetAccountId();
        }
    }
}
