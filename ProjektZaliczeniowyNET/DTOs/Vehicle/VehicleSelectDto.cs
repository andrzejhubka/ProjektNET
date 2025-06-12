namespace ProjektZaliczeniowyNET.DTOs.Vehicle
{
    public class VehicleSelectDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string LicensePlate { get; set; } = null!;
        public int CustomerId { get; set; }
    }
}