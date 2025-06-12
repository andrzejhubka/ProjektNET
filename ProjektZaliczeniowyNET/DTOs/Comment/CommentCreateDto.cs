using System.ComponentModel.DataAnnotations;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.Comment
{
    public class CommentCreateDto
    {
        [Required(ErrorMessage = "Treść komentarza jest wymagana")]
        [StringLength(2000, ErrorMessage = "Treść komentarza nie może być dłuższa niż 2000 znaków")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Typ komentarza jest wymagany")]
        public CommentType Type { get; set; }

        [Required(ErrorMessage = "Zlecenie serwisowe jest wymagane")]
        public int ServiceOrderId { get; set; }
    }
}