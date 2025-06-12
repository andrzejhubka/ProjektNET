using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using System.Collections.Generic;

public interface IVehicleMapper
{
    VehicleDto ToDto(Vehicle vehicle);
    Vehicle ToEntity(VehicleCreateDto createDto);
    void UpdateEntity(Vehicle vehicle, UpdateVehicleDto updateDto);
    IEnumerable<VehicleListDto> ToListDto(IEnumerable<Vehicle> vehicles);
}