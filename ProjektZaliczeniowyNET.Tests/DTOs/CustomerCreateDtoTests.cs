// ProjektZaliczeniowyNET.Tests/DTOs/CustomerDtoTests.cs
using NUnit.Framework;
using ProjektZaliczeniowyNET.DTOs.Customer;
using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.Tests.DTOs
{
    [TestFixture]
    public class CustomerDtoTests
    {
        [Test]
        public void CustomerDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new CustomerDto
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan@example.com",
                PhoneNumber = "123456789"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void CustomerDto_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var dto = new CustomerDto
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "test@example.com"
            };

            // Assert
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.FirstName, Is.EqualTo("Jan"));
            Assert.That(dto.LastName, Is.EqualTo("Kowalski"));
            Assert.That(dto.Email, Is.EqualTo("test@example.com"));
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