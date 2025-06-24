using NUnit.Framework;
using Moq;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjektZaliczeniowyNET.Tests.Services
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private CustomerService _service;
        private ApplicationDbContext _context;
        private Mock<CustomerMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mapperMock = new Mock<CustomerMapper>();
            _service = new CustomerService(_context, _mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            var result = await _service.GetCustomerByIdAsync(999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCustomerByIdAsync_ShouldReturnDto_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan@example.com",
                PhoneNumber = "123456789"
            };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetCustomerByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.FirstName, Is.EqualTo("Jan"));
            Assert.That(result.LastName, Is.EqualTo("Kowalski"));
        }


        [Test]
        public async Task CreateCustomerAsync_ShouldAddCustomerAndReturnDto()
        {
            // Arrange
            var createDto = new CustomerCreateDto
            {
                FirstName = "Anna",
                LastName = "Nowak",
                Email = "anna@example.com",
                PhoneNumber = "987654321"
            };

            // Act
            var result = await _service.CreateCustomerAsync(createDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Anna"));
            Assert.That(result.LastName, Is.EqualTo("Nowak"));

            var customerInDb = await _context.Customers.FindAsync(result.Id);
            Assert.That(customerInDb, Is.Not.Null);
            Assert.That(customerInDb.FirstName, Is.EqualTo("Anna"));
        }


        [Test]
        public async Task UpdateCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            var updateDto = new CustomerUpdateDto { FirstName = "Updated" };

            var result = await _service.UpdateCustomerAsync(999, updateDto);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateCustomerAsync_ShouldUpdateAndReturnDto_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan@example.com",
                PhoneNumber = "123456789"
            };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            var updateDto = new CustomerUpdateDto
            {
                FirstName = "Janusz",
                LastName = "Kowalski",
                Email = "jan@example.com",
                PhoneNumber = "123456789"
            };

            // Act
            var result = await _service.UpdateCustomerAsync(1, updateDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Janusz"));

            var customerInDb = await _context.Customers.FindAsync(1);
            Assert.That(customerInDb.FirstName, Is.EqualTo("Janusz"));
        }


        [Test]
        public async Task DeleteCustomerAsync_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            var result = await _service.DeleteCustomerAsync(999);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteCustomerAsync_ShouldSetIsActiveFalseAndReturnTrue_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { Id = 1, IsActive = true };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteCustomerAsync(1);

            Assert.That(result, Is.True);
            var customerInDb = await _context.Customers.FindAsync(1);
            Assert.That(customerInDb.IsActive, Is.False);
        }
    }
}
