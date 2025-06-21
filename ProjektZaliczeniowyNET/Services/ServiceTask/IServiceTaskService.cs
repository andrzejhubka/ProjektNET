using ProjektZaliczeniowyNET.DTOs.ServiceTask;

namespace ProjektZaliczeniowyNET.Interfaces
{
    public interface IServiceTaskService
    {
        Task<IEnumerable<ServiceTaskListDto>> GetAllAsync();
        Task<ServiceTaskDto?> GetByIdAsync(int id);
        Task<ServiceTaskDto> CreateAsync(ServiceTaskCreateDto createDto);
        Task<ServiceTaskDto?> UpdateAsync(int id, ServiceTaskUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarkAsCompletedAsync(int id);
        Task<bool> MarkAsNotCompletedAsync(int id);
       
    }
}