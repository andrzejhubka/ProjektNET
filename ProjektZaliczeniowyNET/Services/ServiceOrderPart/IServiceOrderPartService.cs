using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;

namespace ProjektZaliczeniowyNET.Services
{
    public interface IServiceOrderPartService
    {
        Task<ServiceOrderPartDto?> GetByIdAsync(int id);
        Task<IEnumerable<ServiceOrderPartListDto>> GetAllByServiceOrderIdAsync(int serviceOrderId);
        Task<ServiceOrderPartDto> CreateAsync(ServiceOrderPartCreateDto dto);
        Task<bool> UpdateAsync(int id, ServiceOrderPartUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}