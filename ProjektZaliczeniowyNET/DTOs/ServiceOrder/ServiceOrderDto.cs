using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.DTOs.Comment;
using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderDto
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public string? Description { get; set; }
        public ServiceOrderStatus Status { get; set; }
        public string? StatusDisplay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal FinalCost { get; set; }
        public decimal TotalLaborCost { get; set; }
        public decimal TotalPartsCost { get; set; }
        public decimal TotalCost { get; set; }
        public string? CustomerComplaints { get; set; }
        public string? InternalNotes { get; set; }
        public bool IsWarrantyWork { get; set; }

        // Info o kliencie
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }

        // Info o pojeździe
        public int VehicleId { get; set; }
        public string? VehicleDisplayName { get; set; }
        public string? VehicleLicensePlate { get; set; }

        // Info o mechaniku
        public string? AssignedMechanicId { get; set; }
        public string? AssignedMechanicName { get; set; }

        // Info o twórcy
        public string? CreatedByUserId { get; set; }
        public string? CreatedByUserName { get; set; }

        // Zadania, komentarze, części
        public List<ServiceTaskDto> ServiceTasks { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = new();
        public List<ServiceOrderPartDto> ServiceOrderParts { get; set; } = new();

        public int TasksCount { get; set; }
        public int CompletedTasksCount { get; set; }
        public int CommentsCount { get; set; }
        public int PartsCount => ServiceOrderParts.Count;
    }
}
