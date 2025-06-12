namespace ProjektZaliczeniowyNET.DTOs.Customer
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? City { get; set; }
        public bool IsCompany { get; set; }
        public bool IsActive { get; set; }
        public int VehiclesCount { get; set; }
    }
}