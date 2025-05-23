using Moq;
using FluentValidation;
using FluentValidation.Results;
using FluentAssertions;
using Rommanel.Application.Commands;
using Rommanel.Domain.Repositories;
using CustomerApp.Application.Handlers;
using Rommanel.Domain.Entities;

namespace Rommanel.Tests.Handlers
{
    public class CreateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IValidator<CreateCustomerCommand>> _validatorMock;
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _validatorMock = new Mock<IValidator<CreateCustomerCommand>>();

            _handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldReturnGuid()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "John Doe",
                CpfCnpj = "12345678901",
                Email = "john@example.com",
                Phone = "123456789",
                BirthDate = DateTime.Today.AddYears(-30),
                ZipCode = "12345-678",
                Street = "Street 1",
                Number = "123",
                Neighborhood = "Neighborhood",
                City = "City",
                State = "ST"
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.ExistsByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(false);

            _customerRepositoryMock.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBe(Guid.Empty);
            _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.ExistsByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);
            _customerRepositoryMock.Verify(r => r.AddAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateCustomerCommand();

            var failures = new[]
            {
            new ValidationFailure("Name", "Name is required")
        };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DuplicateCpfCnpj_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "John Doe",
                CpfCnpj = "12345678901",
                Email = "john@example.com"
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.ExistsByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Já existe um cliente com este CPF/CNPJ", ex.Message);
        }

        [Fact]
        public async Task Handle_DuplicateEmail_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                Name = "John Doe",
                CpfCnpj = "12345678901",
                Email = "john@example.com"
            };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _customerRepositoryMock.Setup(r => r.ExistsByCpfCnpjAsync(command.CpfCnpj, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(false);

            _customerRepositoryMock.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Já existe um cliente com este Email.", ex.Message);
        }
    }
}
