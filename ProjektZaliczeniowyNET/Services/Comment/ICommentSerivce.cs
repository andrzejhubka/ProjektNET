using ProjektZaliczeniowyNET.DTOs.Comment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjektZaliczeniowyNET.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetCommentsByServiceOrderIdAsync(int serviceOrderId);
        Task<CommentDto?> GetCommentByIdAsync(int id);
        Task<CommentDto> CreateCommentAsync(CommentCreateDto createDto, string authorId);
        Task<bool> UpdateCommentAsync(int id, UpdateCommentDto updateDto);
        Task<bool> DeleteCommentAsync(int id);
    }
}