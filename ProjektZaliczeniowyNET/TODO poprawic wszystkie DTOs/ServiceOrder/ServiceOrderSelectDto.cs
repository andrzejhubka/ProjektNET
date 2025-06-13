using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderSelectDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ServiceOrderStatus Status { get; set; }
        public string CustomerName { get; set; } = null!;
        public string VehicleDisplayName { get; set; } = null!;
    }
}