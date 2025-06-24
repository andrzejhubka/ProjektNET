using NUnit.Framework;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.Mappers;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjektZaliczeniowyNET.Tests.Services
{
    [TestFixture]
    public class VehicleServiceTests
    {
        private VehicleService _service;
        private ApplicationDbContext _context;
        private VehicleMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mapper = new VehicleMapper(); // prawdziwy mapper, bez mockowania
            _service = new VehicleService(_context, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenVehicleNotFound()
        {
            var result = await _service.GetByIdAsync(999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateVehicleAndReturnTrue_WhenVehicleExists()
        {
            // Arrange
            var vehicle = new Vehicle
            {
                Id = 1,
                VIN = "12345678901234567",
                LicensePlate = "XYZ1234",
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                CustomerId = 1,
                FuelType = "Benzyna",
                IsActive = true
            };
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            var updateDto = new UpdateVehicleDto
            {
                VIN = "11111111111111111",
                LicensePlate = vehicle.LicensePlate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                CustomerId = vehicle.CustomerId,
                FuelType = vehicle.FuelType,
                IsActive = vehicle.IsActive
            };

            // Act
            var result = await _service.UpdateAsync(vehicle.Id, updateDto);

            // Assert
            Assert.That(result, Is.True);

            var updatedVehicle = await _context.Vehicles.FindAsync(vehicle.Id);
            Assert.That(updatedVehicle.VIN, Is.EqualTo(updateDto.VIN));
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenVehicleNotFound()
        {
            var result = await _service.DeleteAsync(999);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveVehicleAndReturnTrue_WhenVehicleExists()
        {
            var vehicle = new Vehicle
            {
                Id = 1,
                VIN = "12345678901234567",
                LicensePlate = "XYZ1234",
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                CustomerId = 1,
                FuelType = "Benzyna"
            };
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(vehicle.Id);

            Assert.That(result, Is.True);
            var deletedVehicle = await _context.Vehicles.FindAsync(vehicle.Id);
            Assert.That(deletedVehicle, Is.Null);
        }

        [Test]
        public async Task SetImageUrlAsync_ShouldReturnFalse_WhenVehicleNotFound()
        {
            var result = await _service.SetImageUrlAsync(999, "http://example.com/image.png");
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task SetImageUrlAsync_ShouldUpdateImageUrlAndReturnTrue_WhenVehicleExists()
        {
            var vehicle = new Vehicle
            {
                Id = 1,
                VIN = "12345678901234567",
                LicensePlate = "XYZ1234",
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                CustomerId = 1,
                FuelType = "Benzyna"
            };
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            var newUrl = "http://example.com/image.png";

            var result = await _service.SetImageUrlAsync(vehicle.Id, newUrl);

            Assert.That(result, Is.True);

            var updatedVehicle = await _context.Vehicles.FindAsync(vehicle.Id);
            Assert.That(updatedVehicle.ImageUrl, Is.EqualTo(newUrl));
        }
    }
}
