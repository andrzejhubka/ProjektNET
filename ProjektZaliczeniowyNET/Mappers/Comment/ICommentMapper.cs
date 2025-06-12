using ProjektZaliczeniowyNET.DTOs.Comment;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers
{
    public interface ICommentMapper
    {
        CommentDto ToDto(Comment comment);
        IEnumerable<CommentDto> ToDto(IEnumerable<Comment> comments);
        Comment ToEntity(CommentCreateDto createDto, string authorId);
        void UpdateEntity(Comment comment, UpdateCommentDto updateDto);
        string GetCommentTypeDisplay(CommentType type);
    }
}