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
        Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto);
        Task<bool> UpdateAsync(int id, ServiceOrderUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> GetActiveOrdersCountAsync();
        Task<bool> UpdateStatusAsync(int id, ServiceOrderStatus newStatus);

        Task<IEnumerable<ServiceOrderListDto>>  GetFilteredAsync(int? status, string customer, string vehicle, DateTime? dateFrom, DateTime? dateTo, string mechanicId);
    }
}