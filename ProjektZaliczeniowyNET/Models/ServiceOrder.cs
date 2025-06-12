using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required] 
        [StringLength(20)] 
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime? EstimatedCompletionDate { get; set; }

        [Column(TypeName = "decimal(10,2)")] 
        public decimal EstimatedCost { get; set; }

        [Column(TypeName = "decimal(10,2)")] 
        public decimal FinalCost { get; set; }

        [StringLength(1000)] 
        public string? CustomerComplaints { get; set; }

        [StringLength(1000)] 
        public string? InternalNotes { get; set; }

        public bool IsWarrantyWork { get; set; } = false;

        // Klucze obce
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public string? AssignedMechanicId { get; set; }

        [Required] 
        public string CreatedByUserId { get; set; } = string.Empty;

        // Właściwości nawigacyjne
        [ForeignKey("CustomerId")] 
        public virtual Customer? Customer { get; set; }

        [ForeignKey("VehicleId")] 
        public virtual Vehicle? Vehicle { get; set; }

        [ForeignKey("AssignedMechanicId")] 
        public virtual User? AssignedMechanic { get; set; }

        [ForeignKey("CreatedByUserId")] 
        public virtual User? CreatedByUser { get; set; }

        public virtual ICollection<ServiceTask> ServiceTasks { get; set; } = new List<ServiceTask>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Dodana kolekcja części przypisanych do zlecenia
        public virtual ICollection<ServiceOrderPart> ServiceOrderParts { get; set; } = new List<ServiceOrderPart>();

        // Właściwości obliczane
        public decimal TotalLaborCost => ServiceTasks.Sum(st => st.LaborCost);

        public decimal TotalPartsCost => ServiceOrderParts.Sum(p => p.Cost);

        public decimal TotalCost => TotalLaborCost + TotalPartsCost;
    }
}
