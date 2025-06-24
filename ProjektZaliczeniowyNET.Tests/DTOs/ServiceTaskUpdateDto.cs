// ProjektZaliczeniowyNET.Tests/DTOs/ServiceTaskUpdateDtoTests.cs
using NUnit.Framework;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace ProjektZaliczeniowyNET.Tests.DTOs
{
    [TestFixture]
    public class ServiceTaskUpdateDtoTests
    {
        [Test]
        public void ServiceTaskUpdateDto_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var dto = new ServiceTaskUpdateDto
            {
                Description = "Wymiana oleju",
                LaborHours = 2.5m,
                HourlyRate = 80.00m
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void ServiceTaskUpdateDto_WithEmptyDescription_ShouldFailValidation()
        {
            // Arrange
            var dto = new ServiceTaskUpdateDto
            {
                Description = "",
                LaborHours = 1.0m,
                HourlyRate = 50.00m
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ServiceTaskUpdateDto_WithZeroLaborHours_ShouldFailValidation()
        {
            // Arrange
            var dto = new ServiceTaskUpdateDto
            {
                Description = "Test",
                LaborHours = 0.0m,
                HourlyRate = 50.00m
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ServiceTaskUpdateDto_WithZeroHourlyRate_ShouldFailValidation()
        {
            // Arrange
            var dto = new ServiceTaskUpdateDto
            {
                Description = "Test",
                LaborHours = 1.0m,
                HourlyRate = 0.0m
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ServiceTaskUpdateDto_ShouldCalculateTotalCost()
        {
            // Arrange
            var dto = new ServiceTaskUpdateDto
            {
                Description = "Test",
                LaborHours = 3.0m,
                HourlyRate = 50.00m
            };

            // Act
            var totalCost = dto.LaborHours * dto.HourlyRate;

            // Assert
            Assert.That(totalCost, Is.EqualTo(150.00m));
        }

        [Test]
        public void ServiceTaskUpdateDto_ShouldToggleCompletion()
        {
            // Arrange
            var dto = new ServiceTaskUpdateDto
            {
                Description = "Test",
                LaborHours = 1.0m,
                HourlyRate = 50.00m,
                IsCompleted = false
            };

            // Act
            dto.IsCompleted = true;

            // Assert
            Assert.That(dto.IsCompleted, Is.True);
        }

        [Test]
        public void ServiceTaskUpdateDto_WithOptionalFields_ShouldWork()
        {
            // Arrange
            var dto = new ServiceTaskUpdateDto
            {
                Description = "Test",
                DetailedDescription = "Szczegóły",
                LaborHours = 1.0m,
                HourlyRate = 50.00m,
                Notes = "Notatki"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults.Count, Is.EqualTo(0));
            Assert.That(dto.DetailedDescription, Is.EqualTo("Szczegóły"));
            Assert.That(dto.Notes, Is.EqualTo("Notatki"));
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
