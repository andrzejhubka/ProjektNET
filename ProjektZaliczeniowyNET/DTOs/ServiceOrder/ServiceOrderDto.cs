using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.DTOs.Mechanic;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderDto
    {
        public int Id { get; set; }
        
        public CustomerDto Customer { get; set; }
    
        public MechanicDto Mechanic { get; set; }
        
        public VehicleServiceOrderDetailsDto Vehicle { get; set; }
        
        public List<ServiceTaskCreateDto> ServiceTasks { get; set; } = new();
        public Models.ServiceOrderStatus Status { get; set; } = Models.ServiceOrderStatus.Pending;
    }
}