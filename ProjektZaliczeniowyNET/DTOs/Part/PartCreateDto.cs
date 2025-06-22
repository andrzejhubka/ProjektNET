using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.DTOs.Part
{
    public class PartCreateDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }
    }
}