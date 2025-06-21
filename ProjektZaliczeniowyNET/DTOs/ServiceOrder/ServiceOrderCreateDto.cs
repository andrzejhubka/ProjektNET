using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderCreateDto
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