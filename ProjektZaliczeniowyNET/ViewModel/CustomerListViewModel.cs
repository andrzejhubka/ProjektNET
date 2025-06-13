using ProjektZaliczeniowyNET.DTOs.Customer;

namespace ProjektZaliczeniowyNET.ViewModels;

public class CustomerListViewModel
{
    public List<CustomerListDto> Customers { get; set; } = new List<CustomerListDto>();
}
