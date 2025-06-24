using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.DTOs.Part;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ProjektZaliczeniowyNET.Tests.Services
{
    [TestFixture]
    public class PartServiceTests
    {
        private ApplicationDbContext _context;
        private PartService _service;
        private PartMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mapper = new PartMapper();
            _service = new PartService(_context, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllParts()
        {
            // Arrange
            var parts = new List<Part>
            {
                new Part { Name = "Part A", UnitPrice = 10.5m, QuantityInStock = 5 },
                new Part { Name = "Part B", UnitPrice = 15.0m, QuantityInStock = 2 }
            };
            await _context.Parts.AddRangeAsync(parts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(p => p.Name == "Part A"), Is.True);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnPart_WhenExists()
        {
            // Arrange
            var part = new Part { Name = "Test Part", UnitPrice = 20.0m, QuantityInStock = 10 };
            _context.Parts.Add(part);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(part.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Test Part"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_ShouldAddPartToDatabase()
        {
            // Arrange
            var createDto = new PartCreateDto
            {
                Name = "New Part",
                Description = "Some description",
                UnitPrice = 9.99m,
                QuantityInStock = 100
            };

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("New Part"));

            var saved = await _context.Parts.FirstOrDefaultAsync(p => p.Name == "New Part");
            Assert.That(saved, Is.Not.Null);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdatePart_WhenExists()
        {
            // Arrange
            var part = new Part { Name = "Old Name", UnitPrice = 5m, QuantityInStock = 1 };
            _context.Parts.Add(part);
            await _context.SaveChangesAsync();

            var updateDto = new PartUpdateDto
            {
                Name = "Updated Name",
                Description = "Updated Desc",
                UnitPrice = 99.99m,
                QuantityInStock = 50
            };

            // Act
            var result = await _service.UpdateAsync(part.Id, updateDto);

            // Assert
            Assert.That(result, Is.True);

            var updated = await _context.Parts.FindAsync(part.Id);
            Assert.That(updated!.Name, Is.EqualTo("Updated Name"));
            Assert.That(updated.QuantityInStock, Is.EqualTo(50));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnFalse_WhenPartDoesNotExist()
        {
            var updateDto = new PartUpdateDto
            {
                Name = "Doesn't matter",
                Description = null,
                UnitPrice = 1m,
                QuantityInStock = 1
            };

            var result = await _service.UpdateAsync(999, updateDto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemovePart_WhenExists()
        {
            // Arrange
            var part = new Part { Name = "ToDelete", UnitPrice = 1, QuantityInStock = 1 };
            _context.Parts.Add(part);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteAsync(part.Id);

            // Assert
            Assert.That(result, Is.True);
            var deleted = await _context.Parts.FindAsync(part.Id);
            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenPartNotFound()
        {
            var result = await _service.DeleteAsync(123456);
            Assert.That(result, Is.False);
        }
    }
}
