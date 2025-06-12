using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers;

public interface ICustomerMapper
{
    CustomerDto ToDto(Customer customer);
    CustomerListDto ToListDto(Customer customer);
    CustomerSelectDto ToSelectDto(Customer customer);
    Customer ToEntity(CustomerCreateDto createDto);
    void UpdateEntity(Customer customer, CustomerUpdateDto updateDto);
}