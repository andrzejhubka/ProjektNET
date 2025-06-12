using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderSelectDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string Description { get; set; }
        public ServiceOrderStatus Status { get; set; }
        public string CustomerName { get; set; }
        public string VehicleDisplayName { get; set; }
    }
}