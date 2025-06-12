using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.DTOs.Customer
{
    public class CustomerCreateDto
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [StringLength(100, ErrorMessage = "Email nie może być dłuższy niż 100 znaków")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [StringLength(20, ErrorMessage = "Numer telefonu nie może być dłuższy niż 20 znaków")]
        [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu")]
        public string PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Adres nie może być dłuższy niż 200 znaków")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "Miasto nie może być dłuższe niż 50 znaków")]
        public string? City { get; set; }

        [StringLength(10, ErrorMessage = "Kod pocztowy nie może być dłuższy niż 10 znaków")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi być w formacie XX-XXX")]
        public string? PostalCode { get; set; }

        [StringLength(11, ErrorMessage = "NIP nie może być dłuższy niż 11 znaków")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "NIP musi składać się z 10 cyfr")]
        public string? TaxNumber { get; set; }

        public bool IsCompany { get; set; }

        [StringLength(100, ErrorMessage = "Nazwa firmy nie może być dłuższa niż 100 znaków")]
        public string? CompanyName { get; set; }

        [StringLength(500, ErrorMessage = "Notatki nie mogą być dłuższe niż 500 znaków")]
        public string? Notes { get; set; }
    }
}