using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjektZaliczeniowyNET.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleListDto>> GetAllAsync();
        Task<VehicleDto?> GetByIdAsync(int id);
        Task<VehicleDto> CreateAsync(VehicleCreateDto dto);
        Task<bool> UpdateAsync(int id, UpdateVehicleDto dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> SetImageUrlAsync(int id, string imageUrl);
    }
}