namespace ProjektZaliczeniowyNET.DTOs.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? TaxNumber { get; set; }
        public bool IsCompany { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public int VehiclesCount { get; set; }
        public int ServiceOrdersCount { get; set; }
    }
}