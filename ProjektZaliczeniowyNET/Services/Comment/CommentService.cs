using ProjektZaliczeniowyNET.DTOs.Comment;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjektZaliczeniowyNET.Data;

namespace ProjektZaliczeniowyNET.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentMapper _commentMapper;

        public CommentService(ApplicationDbContext context, ICommentMapper commentMapper)
        {
            _context = context;
            _commentMapper = commentMapper;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByServiceOrderIdAsync(int serviceOrderId)
        {
            var comments = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.ServiceOrder)
                .Where(c => c.ServiceOrderId == serviceOrderId)
                .ToListAsync();

            return _commentMapper.ToDto(comments);
        }

        public async Task<CommentDto?> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.ServiceOrder)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
                return null;

            return _commentMapper.ToDto(comment);
        }

        public async Task<CommentDto> CreateCommentAsync(CommentCreateDto createDto, string authorId)
        {
            var comment = _commentMapper.ToEntity(createDto, authorId);

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Załaduj nawigacyjne, aby mapper miał dostęp do Author i ServiceOrder
            await _context.Entry(comment).Reference(c => c.Author).LoadAsync();
            await _context.Entry(comment).Reference(c => c.ServiceOrder).LoadAsync();

            return _commentMapper.ToDto(comment);
        }

        public async Task<bool> UpdateCommentAsync(int id, UpdateCommentDto updateDto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return false;

            _commentMapper.UpdateEntity(comment, updateDto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
