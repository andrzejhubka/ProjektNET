using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        [StringLength(20)]
        public string? EmployeeNumber { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Nawigacje (opcjonalne)
        public virtual ICollection<ServiceOrder>? AssignedOrders { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }

        // Pole wyliczane
        public string FullName => $"{FirstName} {LastName}";
    }
}