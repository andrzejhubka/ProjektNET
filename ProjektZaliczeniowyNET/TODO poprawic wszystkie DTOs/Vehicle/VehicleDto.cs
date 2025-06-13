namespace ProjektZaliczeniowyNET.DTOs.Vehicle
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string VIN { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Color { get; set; }
        public string? EngineNumber { get; set; }
        public int? Mileage { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string DisplayName { get; set; } = string.Empty;

        // Informacje o właścicielu
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        // Statystyki
        public int ServiceOrdersCount { get; set; }
    }
}