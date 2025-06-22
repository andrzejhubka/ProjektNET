using System.ComponentModel.DataAnnotations;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderUpdateDto
    {
        [Required] 
        public int VehicleId { get; set; }

        [Required] 
        public int CustomerId { get; set; }

        [Required]
        public string? AssignedMechanicId { get; set; }

        public List<ServiceTaskCreateDto> ServiceTasks { get; set; } = new();
        
        public Models.ServiceOrderStatus Status { get; set; } = Models.ServiceOrderStatus.Pending;
    }
}