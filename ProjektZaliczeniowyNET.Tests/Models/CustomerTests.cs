// ProjektZaliczeniowyNET.Tests/Models/CustomerTests.cs
using NUnit.Framework;
using ProjektZaliczeniowyNET.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace ProjektZaliczeniowyNET.Tests.Models
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void Customer_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@example.com",
                PhoneNumber = "123456789",
                Address = "ul. Testowa 123",
                City = "Warszawa",
                PostalCode = "00-001"
            };

            // Act
            var validationResults = ValidateModel(customer);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void Customer_ShouldInitializeVehiclesList()
        {
            // Arrange & Act
            var customer = new Customer();

            // Assert
            Assert.That(customer.Vehicles, Is.Not.Null);
            Assert.That(customer.Vehicles, Is.Empty);
        }

        [Test]
        public void Customer_ShouldAddVehicleCorrectly()
        {
            // Arrange
            var customer = new Customer();
            var vehicle = new Vehicle { Make = "Toyota", Model = "Corolla" };

            // Act
            customer.Vehicles.Add(vehicle);

            // Assert
            Assert.That(customer.Vehicles.Count, Is.EqualTo(1));
            Assert.That(customer.Vehicles.First().Make, Is.EqualTo("Toyota"));
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
