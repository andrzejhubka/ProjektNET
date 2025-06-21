using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderListDto
    {
        public int Id { get; set; }

        public string CustomerFullName { get; set; } = string.Empty;

        public string VehicleDisplayName { get; set; } = string.Empty;

        public string AssignedMechanicEmail { get; set; } = string.Empty;

        public ServiceOrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}