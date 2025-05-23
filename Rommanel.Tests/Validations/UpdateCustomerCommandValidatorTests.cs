using FluentAssertions;
using Rommanel.Application.Commands;
using Rommanel.Application.Validators;

namespace Rommanel.Tests.Validations
{
    public class UpdateCustomerCommandValidatorTests
    {
        private readonly UpdateCustomerCommandValidator _validator;

        public UpdateCustomerCommandValidatorTests()
        {
            _validator = new UpdateCustomerCommandValidator();
        }

        [Fact]
        public void Validate_InvalidEmail_ShouldFail()
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                CpfCnpj = "12345678901",
                Email = "invalid-email",
                Phone = "123456789",
                BirthDate = DateTime.Today.AddYears(-20),
                IsIndividual = true,
                StateRegistration = null,
                IsStateRegistrationExempt = false,
                ZipCode = "12345-678",
                Street = "Street",
                Number = "123",
                Neighborhood = "Neighborhood",
                City = "City",
                State = "ST"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void Validate_EmptyName_ShouldFail()
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                Id = Guid.NewGuid(),
                Name = "",
                CpfCnpj = "12345678901",
                Email = "valid@example.com",
                Phone = "123456789",
                BirthDate = DateTime.Today.AddYears(-20),
                IsIndividual = true,
                StateRegistration = null,
                IsStateRegistrationExempt = false,
                ZipCode = "12345-678",
                Street = "Street",
                Number = "123",
                Neighborhood = "Neighborhood",
                City = "City",
                State = "ST"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_EmptyCpfCnpj_ShouldFail()
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                CpfCnpj = "",
                Email = "valid@example.com",
                Phone = "123456789",
                BirthDate = DateTime.Today.AddYears(-20),
                IsIndividual = true,
                StateRegistration = null,
                IsStateRegistrationExempt = false,
                ZipCode = "12345-678",
                Street = "Street",
                Number = "123",
                Neighborhood = "Neighborhood",
                City = "City",
                State = "ST"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CpfCnpj");
        }

        [Fact]
        public void Validate_ValidCommand_ShouldPass()
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                CpfCnpj = "12345678901",
                Email = "valid@example.com",
                Phone = "123456789",
                BirthDate = DateTime.Today.AddYears(-20),
                IsIndividual = true,
                StateRegistration = null,
                IsStateRegistrationExempt = false,
                ZipCode = "12345-678",
                Street = "Street",
                Number = "123",
                Neighborhood = "Neighborhood",
                City = "City",
                State = "ST"
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }

}
