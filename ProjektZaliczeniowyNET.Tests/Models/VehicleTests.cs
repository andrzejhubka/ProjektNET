// ProjektZaliczeniowyNET.Tests/Models/VehicleTests.cs
using NUnit.Framework;
using ProjektZaliczeniowyNET.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProjektZaliczeniowyNET.Tests.Models
{
    [TestFixture]
    public class VehicleTests
    {
        [Test]
        public void Vehicle_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var vehicle = new Vehicle
            {
                Id = 1,
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                LicensePlate = "ABC123",
                VIN = "1HGBH41JXMN109186",
                CustomerId = 1
            };

            // Act
            var validationResults = ValidateModel(vehicle);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void Vehicle_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var vehicle = new Vehicle
            {
                Make = "Honda",
                Model = "Civic",
                Year = 2019
            };

            // Assert
            Assert.That(vehicle.Make, Is.EqualTo("Honda"));
            Assert.That(vehicle.Model, Is.EqualTo("Civic"));
            Assert.That(vehicle.Year, Is.EqualTo(2019));
        }

        [Test]
        public void Vehicle_ShouldInitializeServiceOrdersList()
        {
            // Arrange & Act
            var vehicle = new Vehicle();

            // Assert
            Assert.That(vehicle.ServiceOrders, Is.Not.Null);
            Assert.That(vehicle.ServiceOrders, Is.Empty);
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
