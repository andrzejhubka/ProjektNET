using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProjektZaliczeniowyNET.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        
        public ApplicationRole(string roleName) : base(roleName) { }

        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // Stałe dla ról
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Mechanik = "Mechanik";
        public const string Recepcjonista = "Recepcjonista";
    }
}