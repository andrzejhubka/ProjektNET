using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.DTOs.Vehicle;

namespace ProjektZaliczeniowyNET.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerListDto>> GetAllCustomersAsync(string? search = null);
        Task<IEnumerable<CustomerSelectDto>> GetCustomersForSelectAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto createDto);
        Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerUpdateDto updateDto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<bool> CustomerExistsAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
        Task<IEnumerable<VehicleDto>?> GetVehiclesForCustomerAsync(int customerId);
    }
}