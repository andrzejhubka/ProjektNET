using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderListDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ServiceOrderStatus Status { get; set; }
        public string StatusDisplay { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal TotalCost { get; set; }
        public string CustomerName { get; set; } = null!;
        public string VehicleDisplayName { get; set; } = null!;
        public string? AssignedMechanicName { get; set; }
        public int TasksCount { get; set; }
        public int CompletedTasksCount { get; set; }
        public bool IsWarrantyWork { get; set; }
    }
}