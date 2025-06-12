using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjektZaliczeniowyNET.Mappers
{
    public class VehicleMapper : IVehicleMapper
    {
        public VehicleDto ToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                VIN = vehicle.VIN,
                LicensePlate = vehicle.LicensePlate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color,
                EngineNumber = vehicle.EngineNumber,
                Mileage = vehicle.Mileage,
                FuelType = vehicle.FuelType,
                ImageUrl = vehicle.ImageUrl,
                Notes = vehicle.Notes,
                CreatedAt = vehicle.CreatedAt,
                IsActive = vehicle.IsActive,
                DisplayName = vehicle.DisplayName,

                CustomerId = vehicle.CustomerId,
                CustomerName = vehicle.Customer?.DisplayName ?? "",
                CustomerEmail = vehicle.Customer?.Email ?? "",
                CustomerPhone = vehicle.Customer?.PhoneNumber ?? "",


                ServiceOrdersCount = vehicle.ServiceOrders?.Count ?? 0
            };
        }

        public VehicleListDto ToListDto(Vehicle vehicle)
        {
            return new VehicleListDto
            {
                Id = vehicle.Id,
                DisplayName = vehicle.DisplayName,
                LicensePlate = vehicle.LicensePlate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                FuelType = vehicle.FuelType,
                Mileage = vehicle.Mileage,
                IsActive = vehicle.IsActive,
                CustomerName = vehicle.Customer?.DisplayName
            };
        }

        public IEnumerable<VehicleListDto> ToListDto(IEnumerable<Vehicle> vehicles)
        {
            return vehicles.Select(ToListDto);
        }

        public Vehicle ToEntity(VehicleCreateDto dto)
        {
            return new Vehicle
            {
                VIN = dto.VIN,
                LicensePlate = dto.LicensePlate,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                Color = dto.Color,
                EngineNumber = dto.EngineNumber,
                Mileage = dto.Mileage,
                FuelType = dto.FuelType,
                ImageUrl = dto.ImageUrl,
                Notes = dto.Notes,
                CustomerId = dto.CustomerId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public void UpdateEntity(Vehicle vehicle, UpdateVehicleDto dto)
        {
            vehicle.VIN = dto.VIN;
            vehicle.LicensePlate = dto.LicensePlate;
            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;
            vehicle.Color = dto.Color;
            vehicle.EngineNumber = dto.EngineNumber;
            vehicle.Mileage = dto.Mileage;
            vehicle.FuelType = dto.FuelType;
            vehicle.ImageUrl = dto.ImageUrl;
            vehicle.Notes = dto.Notes;
            vehicle.IsActive = dto.IsActive;
            vehicle.CustomerId = dto.CustomerId;
        }
    }
}
