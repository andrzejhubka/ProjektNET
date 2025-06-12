using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.DTOs.Vehicle
{
    public class UpdateVehicleDto
    {
        [Required(ErrorMessage = "VIN jest wymagany")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN musi mieć dokładnie 17 znaków")]
        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "VIN zawiera nieprawidłowe znaki")]
        public string VIN { get; set; } = null!;

        [Required(ErrorMessage = "Numer rejestracyjny jest wymagany")]
        [StringLength(15, ErrorMessage = "Numer rejestracyjny nie może być dłuższy niż 15 znaków")]
        public string LicensePlate { get; set; } = null!;

        [Required(ErrorMessage = "Marka jest wymagana")]
        [StringLength(50, ErrorMessage = "Marka nie może być dłuższa niż 50 znaków")]
        public string Make { get; set; } = null!;

        [Required(ErrorMessage = "Model jest wymagany")]
        [StringLength(50, ErrorMessage = "Model nie może być dłuższy niż 50 znaków")]
        public string Model { get; set; } = null!;

        [Required(ErrorMessage = "Rok produkcji jest wymagany")]
        [Range(1900, 2030, ErrorMessage = "Rok produkcji musi być między 1900 a 2030")]
        public int Year { get; set; }

        [StringLength(50, ErrorMessage = "Kolor nie może być dłuższy niż 50 znaków")]
        public string? Color { get; set; }

        [StringLength(20, ErrorMessage = "Numer silnika nie może być dłuższy niż 20 znaków")]
        public string? EngineNumber { get; set; }

        [Range(0, 9999999, ErrorMessage = "Przebieg musi być nieujemny")]
        public int? Mileage { get; set; }

        [Required(ErrorMessage = "Typ paliwa jest wymagany")]
        [StringLength(20, ErrorMessage = "Typ paliwa nie może być dłuższy niż 20 znaków")]
        public string FuelType { get; set; } = null!;

        [StringLength(500, ErrorMessage = "URL obrazu nie może być dłuższy niż 500 znaków")]
        [Url(ErrorMessage = "Nieprawidłowy format URL")]
        public string? ImageUrl { get; set; }

        [StringLength(1000, ErrorMessage = "Notatki nie mogą być dłuższe niż 1000 znaków")]
        public string? Notes { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Klient jest wymagany")]
        public int CustomerId { get; set; }
    }
}
