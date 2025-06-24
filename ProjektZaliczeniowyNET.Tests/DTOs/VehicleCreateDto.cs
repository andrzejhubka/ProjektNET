// ProjektZaliczeniowyNET.Tests/DTOs/VehicleCreateDtoTests.cs
using NUnit.Framework;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.Tests.DTOs
{
    [TestFixture]
    public class VehicleCreateDtoTests
    {
        [Test]
        public void VehicleCreateDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new VehicleCreateDto
            {
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                LicensePlate = "ABC123",
                VIN = "1HGBH41JXMN109186",
                CustomerId = 1
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
        }

        [TestCase("", false, Description = "Empty make should fail")]
        [TestCase("Toyota", true, Description = "Valid make should pass")]
        public void VehicleCreateDto_MakeValidation_ShouldWorkCorrectly(string make, bool shouldPass)
        {
            // Arrange
            var dto = new VehicleCreateDto
            {
                Make = make,
                Model = "Corolla",
                Year = 2020,
                CustomerId = 1
            };

            // Act
            var validationResults = ValidateModel(dto);
            var hasMakeError = validationResults.Any(v => v.MemberNames.Contains("Make"));

            // Assert
            if (shouldPass)
                Assert.That(hasMakeError, Is.False, "Make validation should pass");
            else
                Assert.That(hasMakeError, Is.True, "Make validation should fail");
        }

        [TestCase(1907, true, Description = "Too old year should fail")]
        [TestCase(2000, true, Description = "Valid year should pass")]
        [TestCase(2024, true, Description = "Current year should pass")]
        public void VehicleCreateDto_YearValidation_ShouldWorkCorrectly(int year, bool shouldPass)
        {
            // Arrange
            var dto = new VehicleCreateDto
            {
                Make = "Toyota",
                Model = "Corolla",
                Year = year,
                CustomerId = 1
            };

            // Act
            var validationResults = ValidateModel(dto);
            var hasYearError = validationResults.Any(v => v.MemberNames.Contains("Year"));

            // Assert
            if (shouldPass)
                Assert.That(hasYearError, Is.False, "Year validation should pass");
            else
                Assert.That(hasYearError, Is.True, "Year validation should fail");
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
