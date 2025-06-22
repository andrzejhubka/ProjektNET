using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.Mechanic
{
    public class MechanicDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
    }
}