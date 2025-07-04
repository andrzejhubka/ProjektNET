using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ProjektZaliczeniowyNET.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly VehicleMapper _mapper;

        public VehicleService(ApplicationDbContext dbContext, VehicleMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleListDto>> GetAllAsync()
        {
            var vehicles = await _dbContext.Vehicles
                .Include(v => v.Customer)
                .Include(v => v.ServiceOrders)
                .ToListAsync();

            return _mapper.ToListDto(vehicles);
        }

        public async Task<VehicleDto?> GetByIdAsync(int id)
        {
            var vehicle = await _dbContext.Vehicles
                .Include(v => v.Customer)
                .Include(v => v.ServiceOrders)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null) return null;

            return _mapper.ToDto(vehicle);
        }

        public async Task<VehicleDto> CreateAsync(VehicleCreateDto dto)
        {
            var vehicle = _mapper.ToEntity(dto);
            _dbContext.Vehicles.Add(vehicle);
            await _dbContext.SaveChangesAsync();

            // Reload vehicle with related data for mapping
            await _dbContext.Entry(vehicle).Reference(v => v.Customer).LoadAsync();
            await _dbContext.Entry(vehicle).Collection(v => v.ServiceOrders).LoadAsync();

            return _mapper.ToDto(vehicle);
        }

        public async Task<bool> UpdateAsync(int id, UpdateVehicleDto dto)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);
            if (vehicle == null) return false;

            _mapper.UpdateEntity(vehicle, dto);
            _dbContext.Vehicles.Update(vehicle);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);
            if (vehicle == null) return false;

            _dbContext.Vehicles.Remove(vehicle);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> SetImageUrlAsync(int id, string imageUrl)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);
            if (vehicle == null) return false;

            vehicle.ImageUrl = imageUrl;
            _dbContext.Vehicles.Update(vehicle);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
