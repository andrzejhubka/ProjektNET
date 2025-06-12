using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public CommentType Type { get; set; }
        public string TypeDisplay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; }
        public int ServiceOrderId { get; set; }
        public string ServiceOrderNumber { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}