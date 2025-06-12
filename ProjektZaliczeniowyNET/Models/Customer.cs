using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty;

        [StringLength(11)]
        public string? TaxNumber { get; set; } // NIP dla firm

        public bool IsCompany { get; set; } = false;

        [StringLength(100)]
        public string? CompanyName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Właściwości nawigacyjne
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

        // Właściwość obliczana
        public string FullName => $"{FirstName} {LastName}";
        public string DisplayName => IsCompany && !string.IsNullOrEmpty(CompanyName) 
            ? CompanyName 
            : FullName;
    }
}