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

        // Właściwości specyficzne dla warsztatu
        public string? EmployeeNumber { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Właściwości nawigacyjne - relacje z innymi encjami
        public virtual ICollection<ServiceOrder> AssignedOrders { get; set; } = new List<ServiceOrder>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Właściwość obliczana
        public string FullName => $"{FirstName} {LastName}";
    }
}