using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;

namespace ProjektZaliczeniowyNET.Models
{
    public enum ServiceOrderStatus
    {
        Pending = 0, // Oczekujące
        InProgress = 1, // W trakcie
        Completed = 2, // Zakończone
        Cancelled = 3, // Anulowane
        WaitingForParts = 4 // Oczekiwanie na części
    }
    
    public class ServiceOrder
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public string AssignedMechanicId { get; set; }
  
        public Vehicle? Vehicle { get; set; }
        public Customer? Customer { get; set; }
        public IdentityUser AssignedMechanic { get; set; }
        
        public required ServiceOrderStatus  Status { get; set; }  
        
        public List<ServiceTask> ServiceTasks { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
