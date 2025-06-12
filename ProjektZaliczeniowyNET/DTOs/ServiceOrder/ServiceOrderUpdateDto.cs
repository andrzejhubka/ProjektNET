using System.ComponentModel.DataAnnotations;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderUpdateDto
    {
        [Required(ErrorMessage = "Opis jest wymagany")]
        [StringLength(1000, ErrorMessage = "Opis nie może być dłuższy niż 1000 znaków")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status jest wymagany")]
        public ServiceOrderStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Szacowany koszt musi być nieujemny")]
        public decimal EstimatedCost { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Koszt końcowy musi być nieujemny")]
        public decimal FinalCost { get; set; }

        [StringLength(1000, ErrorMessage = "Skargi klienta nie mogą być dłuższe niż 1000 znaków")]
        public string? CustomerComplaints { get; set; }

        [StringLength(1000, ErrorMessage = "Notatki wewnętrzne nie mogą być dłuższe niż 1000 znaków")]
        public string? InternalNotes { get; set; }

        public bool IsWarrantyWork { get; set; }

        public string? AssignedMechanicId { get; set; }
    }
}