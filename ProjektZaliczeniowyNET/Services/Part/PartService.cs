using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.DTOs.Part;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjektZaliczeniowyNET.Services
{
    public class PartService : IPartService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PartMapper _mapper;

        public PartService(ApplicationDbContext dbContext, PartMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PartListDto>> GetAllAsync()
        {
            var parts = await _dbContext.Parts.ToListAsync();
            return parts.Select(_mapper.ToListDto);
        }

        public async Task<PartDto?> GetByIdAsync(int id)
        {
            var part = await _dbContext.Parts.FindAsync(id);
            if (part == null) return null;
            return _mapper.ToDto(part);
        }

        public async Task<PartDto> CreateAsync(PartCreateDto dto)
        {
            var part = _mapper.ToEntity(dto);
            _dbContext.Parts.Add(part);
            await _dbContext.SaveChangesAsync();
            return _mapper.ToDto(part);
        }

        public async Task<bool> UpdateAsync(int id, PartUpdateDto dto)
        {
            var part = await _dbContext.Parts.FindAsync(id);
            if (part == null) return false;

            _mapper.UpdateEntity(part, dto);
            part.UpdatedAt = DateTime.UtcNow;

            _dbContext.Parts.Update(part);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var part = await _dbContext.Parts.FindAsync(id);
            if (part == null) return false;

            _dbContext.Parts.Remove(part);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
