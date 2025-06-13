using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.DTOs.Comment;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers;

[Mapper]
public partial class CommentMapper
{
    public partial CommentDto ToDto(Comment comment);
    public partial IEnumerable<CommentDto> ToDto(IEnumerable<Comment> comments);

    public partial Comment ToEntity(CommentCreateDto createDto, string authorId);

    public partial void UpdateEntity(Comment comment, UpdateCommentDto updateDto);
}
