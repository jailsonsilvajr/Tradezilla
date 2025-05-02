using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface ISignUp
    {
        Task<Guid> SingUpAsync(SignUpDto signUpDto);
    }
}
