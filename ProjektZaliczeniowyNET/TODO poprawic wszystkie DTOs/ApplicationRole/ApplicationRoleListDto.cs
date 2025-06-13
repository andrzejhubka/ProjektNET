namespace ProjektZaliczeniowyNET.DTOs.Role
{
    public class ApplicationRoleListDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}