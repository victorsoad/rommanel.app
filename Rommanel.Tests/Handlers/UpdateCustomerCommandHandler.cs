using Moq;
using FluentValidation;
using FluentValidation.Results;
using FluentAssertions;
using Rommanel.Application.Commands;
using Rommanel.Application.Handlers;
using Rommanel.Domain.Repositories;
using Rommanel.Domain.Entities;

namespace Rommanel.Tests.Handlers
{
    public class UpdateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IValidator<UpdateCustomerCommand>> _validatorMock;
        private readonly UpdateCustomerCommandHandler _handler;

        public UpdateCustomerCommandHandlerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _validatorMock = new Mock<IValidator<UpdateCustomerCommand>>();
            _handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldUpdateCustomer()
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Name",
                CpfCnpj = "12345678901",
                Email = "updated@example.com",
                Phone = "987654321",
                BirthDate = DateTime.Today.AddYears(-25),
                IsIndividual = true,
                StateRegistration = null,
                IsStateRegistrationExempt = false,
                ZipCode = "12345-678",
                Street = "Updated Street",
                Number = "321",
                Neighborhood = "New Neighborhood",
                City = "New City",
                State = "NS"
            };

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

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(existingCustomer);

            _customerRepositoryMock.Setup(r => r.ExistsByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(false);

            _customerRepositoryMock.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(false);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.ExistsByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.UpdateAsync(existingCustomer, It.IsAny<CancellationToken>()), Times.Once);

            // Also verify that the entity's properties were updated (basic check)
            existingCustomer.Name.Should().Be(command.Name);
            existingCustomer.Email.Should().Be(command.Email);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ShouldThrowValidationException()
        {
            var command = new UpdateCustomerCommand();
            var failures = new[] { new ValidationFailure("Id", "Id is required") };
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(failures));

            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NonExistingCustomer_ShouldThrowKeyNotFoundException()
        {
            var command = new UpdateCustomerCommand { Id = Guid.NewGuid() };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync((CustomerEntity?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }

}
