namespace ProjektZaliczeniowyNET.DTOs.Vehicle
{
    public class VehicleListDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string LicensePlate { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string FuelType { get; set; } = null!;
        public int? Mileage { get; set; }
        public bool IsActive { get; set; }
        public string? CustomerName { get; set; }
        public int ServiceOrdersCount { get; set; }
    }
}