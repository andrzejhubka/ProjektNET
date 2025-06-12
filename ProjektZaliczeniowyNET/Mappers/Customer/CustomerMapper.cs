using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers;
public class CustomerMapper : ICustomerMapper
{
    public CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            City = customer.City,
            PostalCode = customer.PostalCode,
            CreatedAt = customer.CreatedAt,
            IsActive = customer.IsActive,
            Notes = customer.Notes,
            VehiclesCount = customer.Vehicles?.Count ?? 0,
            ServiceOrdersCount = customer.ServiceOrders?.Count ?? 0
        };
    }

    public CustomerListDto ToListDto(Customer customer)
    {
        return new CustomerListDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            City = customer.City,
            IsActive = customer.IsActive,
            VehiclesCount = customer.Vehicles?.Count ?? 0
        };
    }

    public CustomerSelectDto ToSelectDto(Customer customer)
    {
        return new CustomerSelectDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }

    public Customer ToEntity(CustomerCreateDto createDto)
    {
        return new Customer
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            PhoneNumber = createDto.PhoneNumber,
            Address = createDto.Address ?? string.Empty,
            City = createDto.City ?? string.Empty,
            PostalCode = createDto.PostalCode ?? string.Empty,
            Notes = createDto.Notes,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void UpdateEntity(Customer customer, CustomerUpdateDto updateDto)
    {
        customer.FirstName = updateDto.FirstName;
        customer.LastName = updateDto.LastName;
        customer.Email = updateDto.Email;
        customer.PhoneNumber = updateDto.PhoneNumber;
        customer.Address = updateDto.Address ?? string.Empty;
        customer.City = updateDto.City ?? string.Empty;
        customer.PostalCode = updateDto.PostalCode ?? string.Empty;
        customer.IsActive = updateDto.IsActive;
        customer.Notes = updateDto.Notes;
    }
}