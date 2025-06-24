// ProjektZaliczeniowyNET.Tests/Services/ServiceOrderServiceTests.cs
using NUnit.Framework;
using Moq;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;

namespace ProjektZaliczeniowyNET.Tests.Services
{
    [TestFixture]
    public class ServiceOrderServiceTests
    {
        private ServiceOrderService _service;
        private ApplicationDbContext _context;
        private Mock<ServiceOrderMapper> _mapperMock;
        private Mock<IServiceTaskService> _taskServiceMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mapperMock = new Mock<ServiceOrderMapper>();
            _taskServiceMock = new Mock<IServiceTaskService>();
            _service = new ServiceOrderService(_context, _mapperMock.Object, _taskServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        
        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
