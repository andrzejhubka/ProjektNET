using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.DTOs.ServiceTask
{
    public class ServiceTaskCreateDto
    {
        [Required(ErrorMessage = "Opis jest wymagany")]
        [StringLength(200, ErrorMessage = "Opis nie może być dłuższy niż 200 znaków")]
        public string Description { get; set; }

        [StringLength(1000, ErrorMessage = "Szczegółowy opis nie może być dłuższy niż 1000 znaków")]
        public string? DetailedDescription { get; set; }

        [Required(ErrorMessage = "Liczba godzin pracy jest wymagana")]
        [Range(0.1, 999.99, ErrorMessage = "Liczba godzin pracy musi być między 0.1 a 999.99")]
        public decimal LaborHours { get; set; }

        [Required(ErrorMessage = "Stawka godzinowa jest wymagana")]
        [Range(0.01, 9999.99, ErrorMessage = "Stawka godzinowa musi być między 0.01 a 9999.99")]
        public decimal HourlyRate { get; set; }

        [StringLength(500, ErrorMessage = "Notatki nie mogą być dłuższe niż 500 znaków")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Zlecenie serwisowe jest wymagane")]
        public int ServiceOrderId { get; set; }
    }
}