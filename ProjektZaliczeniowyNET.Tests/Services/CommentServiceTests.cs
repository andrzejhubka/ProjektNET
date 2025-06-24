using NUnit.Framework;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.Mappers; // Mapperly mapper
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.Comment;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ProjektZaliczeniowyNET.Tests.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        private CommentService _service;
        private ApplicationDbContext _context;
        private CommentMapper _mapper; // bez mocka!

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            
            _mapper = new CommentMapper(); // lub np. zależnie od implementacji mappera Mapperly

            _service = new CommentService(_context, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task GetCommentByIdAsync_ShouldReturnNull_WhenCommentDoesNotExist()
        {
            // Act
            var result = await _service.GetCommentByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateCommentAsync_ShouldAddCommentAndReturnDto()
        {
            // Arrange
            var createDto = new CommentCreateDto
            {
                Content = "New Comment",
                Type = CommentType.Internal,
                ServiceOrderId = 1
            };
            var authorId = "author1";

            // Act
            var result = await _service.CreateCommentAsync(createDto, authorId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo(createDto.Content));

            var savedComment = await _context.Comments.FirstOrDefaultAsync(c => c.Content == createDto.Content);
            Assert.That(savedComment, Is.Not.Null);
        }

        [Test]
        public async Task UpdateCommentAsync_ShouldReturnFalse_WhenCommentDoesNotExist()
        {
            var updateDto = new UpdateCommentDto { Content = "Any", Type = CommentType.Internal };

            var result = await _service.UpdateCommentAsync(999, updateDto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteCommentAsync_ShouldReturnTrue_WhenCommentExistsAndIsDeleted()
        {
            // Arrange
            var comment = new Comment 
            { 
                Id = 1, 
                Content = "To delete",
                AuthorId = "test-author"  // wymagane pole
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteCommentAsync(1);

            // Assert
            Assert.That(result, Is.True);

            var deletedComment = await _context.Comments.FindAsync(1);
            Assert.That(deletedComment, Is.Null);
        }

        [Test]
        public async Task DeleteCommentAsync_ShouldReturnFalse_WhenCommentDoesNotExist()
        {
            // Act
            var result = await _service.DeleteCommentAsync(999);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
