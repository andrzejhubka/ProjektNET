using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektZaliczeniowyNET.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(17)]
        public string VIN { get; set; }

        [Required]
        [StringLength(15)]
        public string LicensePlate { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; } // marka

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        public int Year { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(20)]
        public string? EngineNumber { get; set; }

        public int? Mileage { get; set; }

        [StringLength(20)]
        public string FuelType { get; set; } = "Benzyna"; // Benzyna, Diesel, Hybrid, Electric

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Klucze obce
        public int CustomerId { get; set; }

        // Właściwości nawigacyjne
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

        // Właściwość obliczana
        public string DisplayName => $"{Make} {Model} ({Year}) - {LicensePlate}";
    }
}