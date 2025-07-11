using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.Models;

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
        Task UpdateManyAsync(int serviceOrderId, List<ServiceTaskCreateDto> newTasks);
        Task DeleteManyAsync(int serviceOrderId, List<ServiceTask> toDeleteTasks);
    }
}