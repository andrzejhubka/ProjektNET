using System.ComponentModel.DataAnnotations;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public ServiceOrderStatus Status { get; set; }

        public string? AssignedMechanicId { get; set; }

        public List<ServiceTaskUpdateDto> ServiceTasks { get; set; } = new();
    }
}