using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektZaliczeniowyNET.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? DetailedDescription { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal LaborHours { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal LaborCost { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedAt { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Klucz obcy
        public int ServiceOrderId { get; set; }

        // Właściwości nawigacyjne
        [ForeignKey("ServiceOrderId")]
        public virtual ServiceOrder ServiceOrder { get; set; } = null!;

        // Właściwość obliczana
        public decimal TotalTaskCost => LaborCost;
    }
}