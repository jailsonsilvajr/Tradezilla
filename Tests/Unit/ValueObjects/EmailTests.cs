using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [InlineData("joao.silva@gmail.com")]
        [InlineData("ana_maria-1985@empresa.com.br")]
        [InlineData("user123+promo@exemplo.co.uk")]
        [InlineData("first.last@sub.dominio.org")]
        [InlineData("a@b.cc")]
        [InlineData("usuario@localhost")] // válido tecnicamente (especialmente em ambientes internos)
        [InlineData("email@123.123.123.123")] // domínio como IP (válido segundo RFC)
        [InlineData("\"nome incomum\"@dominio.com")] // parte local com aspas (válido)
        [InlineData("user@[192.168.0.1]")] // domínio com IP entre colchetes (válido)
        [InlineData("nome.o'reilly@dominio.net")] // nome com apóstrofo
        public void ShouldCreateValidEmail(string email)
        {
            var emailVo = new Email(email);
            emailVo.GetValue().Should().Be(email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("plainaddress")]
        [InlineData("@semusuario.com")]
        [InlineData("usuario@.com")]
        [InlineData("usuario@@dominio.com")]
        [InlineData(".usuario@dominio.com")]
        [InlineData("usuario@domi nio.com")]
        [InlineData("usuario@dominio,com")]
        [InlineData("usuario@dominio@com")]
        [InlineData("\"sem fechamento@dominio.com")]
        [InlineData("usuario@.123.456.789.000")]
        public void ShouldThrowValidationExceptionForInvalidEmail(string? email)
        {
            Action action = () => new Email(email!);
            action.Should().Throw<ValidationException>();
        }
    }
}
