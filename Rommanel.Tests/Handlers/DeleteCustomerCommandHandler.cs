using Moq;
using FluentValidation;
using FluentValidation.Results;
using Rommanel.Application.Commands;
using Rommanel.Domain.Repositories;
using Rommanel.Domain.Entities;
using Rommanel.Application.Handlers;
using System.Net;

namespace Rommanel.Tests.Handlers
{
    public class DeleteCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IValidator<DeleteCustomerCommand>> _validatorMock;
        private readonly DeleteCustomerCommandHandler _handler;

        public DeleteCustomerCommandHandlerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _validatorMock = new Mock<IValidator<DeleteCustomerCommand>>();
            _handler = new DeleteCustomerCommandHandler(_customerRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldDeleteCustomer()
        {
            var id = Guid.NewGuid();
            var command = new DeleteCustomerCommand(id);

            var address = new AddressEntity(
                zipCode: "12345-678",
                street: "Rua Antiga",
                number: "100",
                neighborhood: "Bairro Velho",
                city: "Cidade Velha",
                state: "ST"
            );

            var existingCustomer = new CustomerEntity(
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

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(existingCustomer);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.DeleteAsync(existingCustomer, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingCustomer_ShouldThrowKeyNotFoundException()
        {
            var id = Guid.NewGuid();
            var command = new DeleteCustomerCommand(id);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync((CustomerEntity?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}