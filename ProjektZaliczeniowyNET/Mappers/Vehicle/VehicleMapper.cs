using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Models;
using System.Collections.Generic;

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class VehicleMapper
    {
        // Mapowanie z VehicleCreateDto na encję Vehicle (do tworzenia)
        public partial Vehicle ToEntity(VehicleCreateDto dto);

        // Mapowanie z Vehicle na VehicleDto (szczegóły)
        public partial VehicleDto ToDto(Vehicle vehicle);

        // Mapowanie listy Vehicle na listę VehicleListDto
        public partial IEnumerable<VehicleListDto> ToListDto(IEnumerable<Vehicle> vehicles);

        // Aktualizacja encji Vehicle na podstawie UpdateVehicleDto
        public partial void UpdateEntity(Vehicle entity, UpdateVehicleDto dto);
    }
}