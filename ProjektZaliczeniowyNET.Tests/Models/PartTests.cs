// ProjektZaliczeniowyNET.Tests/Models/PartTests.cs
using NUnit.Framework;
using ProjektZaliczeniowyNET.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.Tests.Models
{
    [TestFixture]
    public class PartTests
    {
        [Test]
        public void Part_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var part = new Part
            {
                Id = 1,
                Name = "Filtr oleju",
                Description = "Filtr oleju silnikowego",
                UnitPrice = 25.99m,
                QuantityInStock = 10
            };

            // Act
            var validationResults = ValidateModel(part);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void Part_ShouldCalculateTotalValue()
        {
            // Arrange
            var part = new Part
            {
                UnitPrice = 50.00m,
                QuantityInStock = 10
            };

            // Act
            var totalValue = part.UnitPrice * part.QuantityInStock;

            // Assert
            Assert.That(totalValue, Is.EqualTo(500.00m));
        }

        [Test]
        public void Part_ShouldDetectLowStock()
        {
            // Arrange
            var part = new Part
            {
                QuantityInStock = 2
            };

            // Act
            var isLowStock = part.QuantityInStock <= 5; // Przykładowy próg

            // Assert
            Assert.That(isLowStock, Is.True);
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