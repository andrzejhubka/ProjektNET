using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;
using ProjektZaliczeniowyNET.Mappers;

namespace ProjektZaliczeniowyNET.Services
{
    public class ServiceOrderPartService : IServiceOrderPartService
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceOrderPartMapper _mapper;

        public ServiceOrderPartService(ApplicationDbContext context, ServiceOrderPartMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceOrderPartDto?> GetByIdAsync(int id)
        {
            var entity = await _context.ServiceOrderParts
                .Include(p => p.Part)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                return null;

            return _mapper.ToDto(entity);
        }

        public async Task<IEnumerable<ServiceOrderPartListDto>> GetAllByServiceOrderIdAsync(int serviceOrderId)
        {
            var entities = await _context.ServiceOrderParts
                .Include(p => p.Part)
                .Where(p => p.ServiceOrderId == serviceOrderId)
                .ToListAsync();

            return entities.Select(e => _mapper.ToListDto(e));
        }

        public async Task<ServiceOrderPartDto> CreateAsync(ServiceOrderPartCreateDto dto)
        {
            // Znajdź Part po nazwie, jeśli nie ma, rzuć wyjątek lub zwróć null
            var part = await _context.Parts.FirstOrDefaultAsync(p => p.Name == dto.PartName);
            if (part == null)
            {
                throw new KeyNotFoundException($"Część o nazwie '{dto.PartName}' nie została znaleziona.");
            }

            var entity = _mapper.ToEntity(dto);
            entity.PartId = part.Id;

            _context.ServiceOrderParts.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.ToDto(entity);
        }

        public async Task<bool> UpdateAsync(int id, ServiceOrderPartUpdateDto dto)
        {
            var entity = await _context.ServiceOrderParts.Include(p => p.Part).FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return false;

            // Jeśli chcesz aktualizować Part po nazwie:
            if (entity.Part.Name != dto.PartName)
            {
                var part = await _context.Parts.FirstOrDefaultAsync(p => p.Name == dto.PartName);
                if (part == null)
                {
                    throw new KeyNotFoundException($"Część o nazwie '{dto.PartName}' nie została znaleziona.");
                }
                entity.PartId = part.Id;
            }

            _mapper.UpdateEntity(entity, dto);

            _context.ServiceOrderParts.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ServiceOrderParts.FindAsync(id);
            if (entity == null) return false;

            _context.ServiceOrderParts.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
