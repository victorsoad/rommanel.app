using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Rommanel.Domain.Entities;
using Rommanel.Infrastructure.Persistence;
using Rommanel.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rommanel.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly CustomerDbContext _context;
        private readonly CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseSqlite("Filename=:memory:") // SQLite In-Memory
                .Options;

            _context = new CustomerDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _repository = new CustomerRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCustomer()
        {
            // Arrange
            var customer = new CustomerEntity(
                name: "Test",
                cpfCnpj: "12345678901",
                birthDate: DateTime.Today.AddYears(-30),
                phone: "123456789",
                email: "test@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("12345-678", "Street", "10", "Neighborhood", "City", "ST")
            );

            // Act
            await _repository.AddAsync(customer, CancellationToken.None);

            // Assert
            var saved = await _context.Customers.FirstOrDefaultAsync(c => c.CpfCnpj == "12345678901");
            saved.Should().NotBeNull();
            saved!.Name.Should().Be("Test");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer()
        {
            // Arrange
            var customer = new CustomerEntity(
                name: "GetTest",
                cpfCnpj: "98765432100",
                birthDate: DateTime.Today.AddYears(-25),
                phone: "987654321",
                email: "gettest@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("54321-000", "Other Street", "20", "Other Neighborhood", "Other City", "OT")
            );

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(customer.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("gettest@example.com");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var customer1 = new CustomerEntity(
                name: "Customer 1",
                cpfCnpj: "55566677788",
                birthDate: DateTime.Today.AddYears(-30),
                phone: "444444444",
                email: "customer1@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("55555-555", "St1", "6", "N1", "C1", "S1")
            );

            var customer2 = new CustomerEntity(
                name: "Customer 2",
                cpfCnpj: "66677788899",
                birthDate: DateTime.Today.AddYears(-40),
                phone: "333333333",
                email: "customer2@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("66666-666", "St2", "7", "N2", "C2", "S2")
            );

            await _context.Customers.AddRangeAsync(customer1, customer2);
            await _context.SaveChangesAsync();

            // Act
            var customers = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            customers.Should().HaveCount(2);
        }


        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new CustomerEntity(
                name: "Old Name",
                cpfCnpj: "11122233344",
                birthDate: DateTime.Today.AddYears(-20),
                phone: "999999999",
                email: "old@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("00000-000", "Old Street", "1", "Old Neighborhood", "Old City", "OS")
            );

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Modifica dados
            customer.Update(
                name: "New Name",
                cpfCnpj: "11122233344",
                birthDate: customer.BirthDate,
                phone: "888888888",
                email: "new@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                zipCode: "11111-111",
                street: "New Street",
                number: "2",
                neighborhood: "New Neighborhood",
                city: "New City",
                state: "NC"                
            );

            // Act
            await _repository.UpdateAsync(customer, CancellationToken.None);

            // Assert
            var updated = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("New Name");
            updated.Email.Should().Be("new@example.com");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCustomer()
        {
            // Arrange
            var customer = new CustomerEntity(
                name: "To Delete",
                cpfCnpj: "22233344455",
                birthDate: DateTime.Today.AddYears(-40),
                phone: "777777777",
                email: "delete@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("22222-222", "Delete St", "3", "Delete Neighborhood", "Delete City", "DC")
            );

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(customer, CancellationToken.None);

            // Assert
            var deleted = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
            deleted.Should().BeNull();
        }

        [Fact]
        public async Task ExistsByCpfCnpjAsync_ShouldReturnTrueIfExists()
        {
            // Arrange
            var customer = new CustomerEntity(
                name: "Exists Test",
                cpfCnpj: "33344455566",
                birthDate: DateTime.Today.AddYears(-35),
                phone: "666666666",
                email: "exists@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("33333-333", "Exists St", "4", "Exists Neighborhood", "Exists City", "EC")
            );

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsByCpfCnpjAsync("33344455566", CancellationToken.None);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByCpfCnpjAsync_ShouldReturnFalseIfNotExists()
        {
            // Act
            var exists = await _repository.ExistsByCpfCnpjAsync("00000000000", CancellationToken.None);

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsByEmailAsync_ShouldReturnTrueIfExists()
        {
            // Arrange
            var customer = new CustomerEntity(
                name: "Email Exists",
                cpfCnpj: "44455566677",
                birthDate: DateTime.Today.AddYears(-28),
                phone: "555555555",
                email: "email_exists@example.com",
                isIndividual: true,
                stateRegistration: null,
                isStateRegistrationExempt: false,
                address: new AddressEntity("44444-444", "Email St", "5", "Email Neighborhood", "Email City", "EC")
            );

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsByEmailAsync("email_exists@example.com", CancellationToken.None);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsByEmailAsync_ShouldReturnFalseIfNotExists()
        {
            // Act
            var exists = await _repository.ExistsByEmailAsync("nonexistent@example.com", CancellationToken.None);

            // Assert
            exists.Should().BeFalse();
        }

    }

}
