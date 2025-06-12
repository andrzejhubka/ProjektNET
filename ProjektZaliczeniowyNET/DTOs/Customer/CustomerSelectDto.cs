namespace ProjektZaliczeniowyNET.DTOs.Customer
{
    public class CustomerSelectDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!; 
        public string PhoneNumber { get; set; } = null!;
    }
}