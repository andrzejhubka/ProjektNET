namespace ProjektZaliczeniowyNET.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
        
    // Właściwość obliczana dla pełnej nazwy
    public string FullName => $"{FirstName} {LastName}";
}
