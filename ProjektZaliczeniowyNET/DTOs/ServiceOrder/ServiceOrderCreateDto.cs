using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrder
{
    public class ServiceOrderCreateDto
    {
        [Required(ErrorMessage = "Opis jest wymagany")]
        [StringLength(1000, ErrorMessage = "Opis nie może być dłuższy niż 1000 znaków")]
        public string Description { get; set; }

        public DateTime? EstimatedCompletionDate { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Szacowany koszt musi być nieujemny")]
        public decimal EstimatedCost { get; set; }

        [StringLength(1000, ErrorMessage = "Skargi klienta nie mogą być dłuższe niż 1000 znaków")]
        public string? CustomerComplaints { get; set; }

        [StringLength(1000, ErrorMessage = "Notatki wewnętrzne nie mogą być dłuższe niż 1000 znaków")]
        public string? InternalNotes { get; set; }

        public bool IsWarrantyWork { get; set; }

        [Required(ErrorMessage = "Klient jest wymagany")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Pojazd jest wymagany")]
        public int VehicleId { get; set; }

        public string? AssignedMechanicId { get; set; }
    }
}