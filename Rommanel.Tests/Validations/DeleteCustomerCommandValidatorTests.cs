using FluentAssertions;
using Rommanel.Application.Commands;
using Rommanel.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rommanel.Tests.Validations
{
    public class DeleteCustomerCommandValidatorTests
    {
        private readonly DeleteCustomerCommandValidator _validator;

        public DeleteCustomerCommandValidatorTests()
        {
            _validator = new DeleteCustomerCommandValidator();
        }

        [Fact]
        public void Validate_EmptyId_ShouldFail()
        {
            // Arrange
            var command = new DeleteCustomerCommand(Guid.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Id é obrigatório.");
        }

        [Fact]
        public void Validate_ValidId_ShouldPass()
        {
            // Arrange
            var command = new DeleteCustomerCommand(Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }

}
