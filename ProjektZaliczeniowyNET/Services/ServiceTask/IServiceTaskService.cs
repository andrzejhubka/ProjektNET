using ProjektZaliczeniowyNET.DTOs.ServiceTask;

namespace ProjektZaliczeniowyNET.Interfaces
{
    public interface IServiceTaskService
    {
        Task<IEnumerable<ServiceTaskListDto>> GetAllAsync();
        Task<IEnumerable<ServiceTaskListDto>> GetByServiceOrderIdAsync(int serviceOrderId);
        Task<ServiceTaskDto?> GetByIdAsync(int id);
        Task<ServiceTaskDto> CreateAsync(ServiceTaskCreateDto createDto);
        Task<ServiceTaskDto?> UpdateAsync(int id, ServiceTaskUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarkAsCompletedAsync(int id);
        Task<bool> MarkAsNotCompletedAsync(int id);
        Task<decimal> GetTotalCostByServiceOrderIdAsync(int serviceOrderId);
        Task<int> GetCompletedTasksCountByServiceOrderIdAsync(int serviceOrderId);
        Task<bool> ExistsAsync(int id);
    }
}