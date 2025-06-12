using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.DTOs.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
        [StringLength(256, ErrorMessage = "Email nie może być dłuższy niż 256 znaków")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu")]
        public string? PhoneNumber { get; set; }

        [StringLength(20)]
        public string? EmployeeNumber { get; set; }

        public bool IsActive { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}