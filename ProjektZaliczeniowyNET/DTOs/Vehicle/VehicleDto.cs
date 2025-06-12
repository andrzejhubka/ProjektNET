namespace ProjektZaliczeniowyNET.DTOs.Vehicle
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        public string? EngineNumber { get; set; }
        public int? Mileage { get; set; }
        public string FuelType { get; set; }
        public string? ImageUrl { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string DisplayName { get; set; }
        
        // Informacje o właścicielu
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        
        // Statystyki
        public int ServiceOrdersCount { get; set; }
    }
}