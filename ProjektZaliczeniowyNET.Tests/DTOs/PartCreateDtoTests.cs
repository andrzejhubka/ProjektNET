using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using ProjektZaliczeniowyNET.DTOs.Part;
using System.Collections.Generic;
using System.Linq;

namespace ProjektZaliczeniowyNET.Tests.DTOs
{
    [TestFixture]
    public class PartCreateDtoTests
    {
        [Test]
        public void PartCreateDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new PartCreateDto
            {
                Name = "Filtr oleju",
                Description = "Filtr oleju silnikowego",
                UnitPrice = 25.99m,
                QuantityInStock = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void PartCreateDto_WithEmptyName_ShouldFailValidation()
        {
            // Arrange
            var dto = new PartCreateDto
            {
                Name = "",
                UnitPrice = 25.99m,
                QuantityInStock = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.GreaterThan(0));
            Assert.That(validationResults.Any(v => v.MemberNames.Contains("Name")), Is.True);
        }

        [Test]
        public void PartCreateDto_WithNegativePrice_ShouldFailValidation()
        {
            // Arrange
            var dto = new PartCreateDto
            {
                Name = "Test Part",
                UnitPrice = -10.00m,
                QuantityInStock = 5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.GreaterThan(0));
            Assert.That(validationResults.Any(v => v.MemberNames.Contains("UnitPrice")), Is.True);
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