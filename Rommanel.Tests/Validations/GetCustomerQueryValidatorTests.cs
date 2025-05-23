using FluentAssertions;
using Rommanel.Application.Queries;
using Rommanel.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rommanel.Tests.Validations
{
    public class GetCustomerByIdQueryValidatorTests
    {
        private readonly GetCustomerQueryValidator _validator;

        public GetCustomerByIdQueryValidatorTests()
        {
            _validator = new GetCustomerQueryValidator();
        }

        [Fact]
        public void Validate_EmptyId_ShouldFail()
        {
            // Arrange
            var query = new GetCustomerQuery(Guid.Empty);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }

        [Fact]
        public void Validate_ValidId_ShouldPass()
        {
            // Arrange
            var query = new GetCustomerQuery(Guid.NewGuid());

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
