using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ProjektZaliczeniowyNET.Services
{
    public interface IServiceOrderService
    {
        Task<ServiceOrderDto?> GetByIdAsync(int id);
        Task<IEnumerable<ServiceOrderListDto>> GetAllAsync();
        Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto, string createdByUserId);
        Task<bool> UpdateAsync(int id, ServiceOrderUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}