using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public CommentType Type { get; set; }
        public string TypeDisplay { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; }
        public int ServiceOrderId { get; set; }
        public string ServiceOrderNumber { get; set; } = null!;
        public string AuthorId { get; set; } = null!; 
        public string AuthorName { get; set; } = null!;
    }
}