using ProjektZaliczeniowyNET.DTOs.Mechanic;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderListDto
    {
        public int Id { get; set; }

        public string CustomerFullName { get; set; } = string.Empty;

        public string VehicleDisplayName { get; set; } = string.Empty;

        public MechanicDto? Mechanic { get; set; }

        public ServiceOrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}