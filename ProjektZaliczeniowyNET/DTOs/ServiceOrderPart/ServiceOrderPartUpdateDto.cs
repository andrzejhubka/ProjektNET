using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowyNET.DTOs.ServiceOrderPart
{
    public class ServiceOrderPartUpdateDto
    {
        [Required(ErrorMessage = "Nazwa części jest wymagana")]
        [StringLength(200, ErrorMessage = "Nazwa części nie może być dłuższa niż 200 znaków")]
        public string PartName { get; set; } = null!;

        [Range(0.01, 999999.99, ErrorMessage = "Koszt części musi być dodatni")]
        public decimal Cost { get; set; }

        [Range(1, 9999, ErrorMessage = "Ilość musi być co najmniej 1")]
        public int Quantity { get; set; }
    }
}