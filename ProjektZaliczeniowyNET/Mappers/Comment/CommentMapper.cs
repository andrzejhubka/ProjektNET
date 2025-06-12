using ProjektZaliczeniowyNET.DTOs.Comment;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers
{
    public class CommentMapper : ICommentMapper
    {
        public CommentDto ToDto(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                Type = comment.Type,
                TypeDisplay = GetCommentTypeDisplay(comment.Type),
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                IsEdited = comment.IsEdited,
                ServiceOrderId = comment.ServiceOrderId,
                ServiceOrderNumber = comment.ServiceOrder?.OrderNumber ?? string.Empty,
                AuthorId = comment.AuthorId,
                AuthorName = $"{comment.Author?.FirstName} {comment.Author?.LastName}".Trim()
            };
        }

        public IEnumerable<CommentDto> ToDto(IEnumerable<Comment> comments)
        {
            return comments.Select(ToDto);
        }

        public Comment ToEntity(CommentCreateDto createDto, string authorId)
        {
            return new Comment
            {
                Content = createDto.Content,
                Type = createDto.Type,
                ServiceOrderId = createDto.ServiceOrderId,
                AuthorId = authorId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void UpdateEntity(Comment comment, UpdateCommentDto updateDto)
        {
            comment.Content = updateDto.Content;
            comment.Type = updateDto.Type;
            comment.UpdatedAt = DateTime.UtcNow;
        }

        public string GetCommentTypeDisplay(CommentType type)
        {
            return type switch
            {
                CommentType.Internal => "Wewnętrzny",
                CommentType.CustomerVisible => "Widoczny dla klienta",
                _ => "Nieznany"
            };
        }
    }
}