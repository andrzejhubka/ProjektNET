using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

namespace ProjektZaliczeniowyNET.Tests.Services
{
    [TestFixture]
    public class ServiceTaskServiceTests
    {
        private ApplicationDbContext _context;
        private ServiceTaskService _service;
        private ServiceTaskMapper _mapper;
        private ILogger<ServiceTaskService> _logger;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mapper = new ServiceTaskMapper();
            _logger = new Mock<ILogger<ServiceTaskService>>().Object;

            _service = new ServiceTaskService(_context, _mapper, _logger);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            var result = await _service.GetByIdAsync(999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_ShouldAddTaskToDatabase()
        {
            var dto = new ServiceTaskCreateDto
            {
                Description = "Diagnostyka",
                LaborCost = 100,
                IsCompleted = false
            };

            var result = await _service.CreateAsync(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Description, Is.EqualTo("Diagnostyka"));

            var inDb = await _context.ServiceTasks.FirstOrDefaultAsync();
            Assert.That(inDb, Is.Not.Null);
            Assert.That(inDb.Description, Is.EqualTo("Diagnostyka"));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnNull_WhenTaskNotFound()
        {
            var dto = new ServiceTaskUpdateDto
            {
                Description = "Doesn't matter",
                    LaborHours = 1,
                    HourlyRate = 2,
                IsCompleted = false
            };

            var result = await _service.UpdateAsync(1234, dto);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveTask_WhenExists()
        {
            var task = new ServiceTask { ServiceOrderId = 1, Description = "ToDelete", LaborCost = 1 };
            _context.ServiceTasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(task.Id);

            Assert.That(result, Is.True);
            var deleted = await _context.ServiceTasks.FindAsync(task.Id);
            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync(999);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task MarkAsCompletedAsync_ShouldMarkTask()
        {
            var task = new ServiceTask { ServiceOrderId = 1, Description = "Complete", LaborCost = 1, IsCompleted = false };
            _context.ServiceTasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _service.MarkAsCompletedAsync(task.Id);

            Assert.That(result, Is.True);
            var updated = await _context.ServiceTasks.FindAsync(task.Id);
            Assert.That(updated.IsCompleted, Is.True);
        }

        [Test]
        public async Task MarkAsNotCompletedAsync_ShouldUnmarkTask()
        {
            var task = new ServiceTask { ServiceOrderId = 1, Description = "Undo", LaborCost = 1, IsCompleted = true };
            _context.ServiceTasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _service.MarkAsNotCompletedAsync(task.Id);

            Assert.That(result, Is.True);
            var updated = await _context.ServiceTasks.FindAsync(task.Id);
            Assert.That(updated.IsCompleted, Is.False);
        }

        [Test]
        public async Task UpdateManyAsync_ShouldReplaceExistingTasks()
        {
            var old = new ServiceTask { ServiceOrderId = 1, Description = "Old Task", LaborCost = 10 };
            _context.ServiceTasks.Add(old);
            await _context.SaveChangesAsync();

            var newTasks = new List<ServiceTaskCreateDto>
            {
                new() { Description = "New Task 1", LaborCost = 100, IsCompleted = false },
                new() { Description = "New Task 2", LaborCost = 200, IsCompleted = true }
            };

            await _service.UpdateManyAsync(1, newTasks);

            var all = await _context.ServiceTasks.Where(t => t.ServiceOrderId == 1).ToListAsync();
            Assert.That(all.Count, Is.EqualTo(2));
            Assert.That(all.Any(t => t.Description == "New Task 1"), Is.True);
            Assert.That(all.Any(t => t.Description == "Old Task"), Is.False);
        }

        [Test]
        public async Task DeleteManyAsync_ShouldRemoveAllTasksForOrder()
        {
            var tasks = new[]
            {
                new ServiceTask { ServiceOrderId = 2, Description = "X" },
                new ServiceTask { ServiceOrderId = 2, Description = "Y" }
            };
            _context.ServiceTasks.AddRange(tasks);
            await _context.SaveChangesAsync();

            await _service.DeleteManyAsync(2, new List<ServiceTask>());

            var remaining = await _context.ServiceTasks.Where(t => t.ServiceOrderId == 2).ToListAsync();
            Assert.That(remaining, Is.Empty);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllWithParts()
        {
            var part = new Part { Name = "Filter", UnitPrice = 10, QuantityInStock = 1 };
            var task = new ServiceTask
            {
                ServiceOrderId = 1,
                Description = "Change filter",
                LaborCost = 25,
                Parts = new List<Part> { part }
            };
            _context.ServiceTasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Description, Is.EqualTo("Change filter"));
        }
    }
}
