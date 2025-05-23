using Moq;
using Rommanel.Application.Queries;
using Rommanel.Domain.Entities;
using Rommanel.Domain.Repositories;

namespace Rommanel.Tests.Handlers
{
    public class GetAllCustomersQueryHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly GetAllCustomersQueryHandler _handler;

        public GetAllCustomersQueryHandlerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _handler = new GetAllCustomersQueryHandler(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllCustomers()
        {
            // Arrange
            var fakeCustomers = new List<CustomerEntity>
            {
                new CustomerEntity(
                    "Customer 1",
                    "12345678901",
                    new DateTime(1990, 1, 1),
                    "1234567890",
                    "customer1@example.com",
                    true,
                    null,
                    false,
                    new AddressEntity("Street 1", "123", "Neighborhood", "City", "State", "00000-000")
                ),
                new CustomerEntity(
                    "Customer 2",
                    "10987654321",
                    new DateTime(1985, 5, 5),
                    "0987654321",
                    "customer2@example.com",
                    true,
                    null,
                    false,
                    new AddressEntity("Street 2", "456", "Neighborhood", "City", "State", "11111-111")
                )
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetAllAsync(1, 10,It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeCustomers);

            var query = new GetAllCustomersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Customers.Count);

            Assert.Contains(result.Customers, c => c.Name == "Customer 1" && c.Email == "customer1@example.com");
            Assert.Contains(result.Customers, c => c.Name == "Customer 2" && c.Email == "customer2@example.com");
        }
    }

}
