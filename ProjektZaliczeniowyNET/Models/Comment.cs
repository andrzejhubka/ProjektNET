using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProjektZaliczeniowyNET.Models
{
    public enum CommentType
    {
        Internal = 0,       // Komentarz wewnętrzny
        CustomerVisible = 1 // Widoczny dla klienta
    }

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = null!;

        public CommentType Type { get; set; } = CommentType.Internal;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsEdited => UpdatedAt.HasValue;

        // Klucze obce
        public int ServiceOrderId { get; set; }
        public string AuthorId { get; set; } = null!;

        // Właściwości nawigacyjne
        [ForeignKey("ServiceOrderId")]
        public virtual ServiceOrder ServiceOrder { get; set; } = null!;

        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; } = null!;
    }
}