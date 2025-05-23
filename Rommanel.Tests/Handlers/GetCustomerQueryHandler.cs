using Moq;
using FluentValidation;
using FluentValidation.Results;
using FluentAssertions;
using Rommanel.Application.Queries;
using Rommanel.Domain.Repositories;
using Rommanel.Domain.Entities;

namespace Rommanel.Tests.Handlers
{
    public class GetCustomerQueryHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IValidator<GetCustomerQuery>> _validatorMock;
        private readonly GetCustomerQueryHandler _handler;

        public GetCustomerQueryHandlerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _validatorMock = new Mock<IValidator<GetCustomerQuery>>();
            _handler = new GetCustomerQueryHandler(_customerRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnCustomerResponse()
        {
            var id = Guid.NewGuid();
            var query = new GetCustomerQuery(id);

            var address = new AddressEntity(
                zipCode: "12345-678",
                street: "Rua Antiga",
                number: "100",
                neighborhood: "Bairro Velho",
                city: "Cidade Velha",
                state: "ST"
            );

            var customerEntity = new CustomerEntity(
                name: "Nome Antigo",
                cpfCnpj: "12345678901",
                birthDate: DateTime.Today.AddYears(-30),
                phone: "111222333",
                email: "old@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address
            );            

            _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(customerEntity);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();            
            result.Name.Should().Be(customerEntity.Name);

            _validatorMock.Verify(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingCustomer_ShouldThrowKeyNotFoundException()
        {
            var id = Guid.NewGuid();
            var query = new GetCustomerQuery(id);

            _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync((CustomerEntity?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }

}
