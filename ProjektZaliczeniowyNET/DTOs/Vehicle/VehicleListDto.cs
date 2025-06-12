namespace ProjektZaliczeniowyNET.DTOs.Vehicle
{
    public class VehicleListDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string FuelType { get; set; }
        public int? Mileage { get; set; }
        public bool IsActive { get; set; }
        public string CustomerName { get; set; }
        public int ServiceOrdersCount { get; set; }
    }
}