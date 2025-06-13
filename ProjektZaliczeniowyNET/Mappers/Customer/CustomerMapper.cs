using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.Models;
using Riok.Mapperly.Abstractions;

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class CustomerMapper
    {
        public partial CustomerDto ToDto(Customer customer);

        public partial CustomerListDto ToListDto(Customer customer);

        public partial CustomerSelectDto ToSelectDto(Customer customer);

        public partial Customer ToEntity(CustomerCreateDto createDto);

        public partial void UpdateEntity([MappingTarget] Customer customer, CustomerUpdateDto updateDto);
    }
}