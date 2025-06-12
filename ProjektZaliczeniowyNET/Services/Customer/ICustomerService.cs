using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerListDto>> GetAllCustomersAsync();
    Task<IEnumerable<CustomerSelectDto>> GetCustomersForSelectAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto createDto);
    Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerUpdateDto updateDto);
    Task<bool> DeleteCustomerAsync(int id);
    Task<bool> CustomerExistsAsync(int id);
    Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
}