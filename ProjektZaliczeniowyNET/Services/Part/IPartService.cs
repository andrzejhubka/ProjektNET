
using ProjektZaliczeniowyNET.DTOs.Part;

namespace ProjektZaliczeniowyNET.Services
{
    public interface IPartService
    {
        Task<IEnumerable<PartListDto>> GetAllAsync();
        Task<PartDto?> GetByIdAsync(int id);
        Task<PartDto> CreateAsync(PartCreateDto dto);
        Task<bool> UpdateAsync(int id, PartUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}