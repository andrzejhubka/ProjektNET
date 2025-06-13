using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.Comment;
using ProjektZaliczeniowyNET.Services;

namespace ProjektZaliczeniowyNET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // zabezpieczenie: tylko zalogowani mogą komentować
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/comment/byServiceOrder/5
        [HttpGet("byServiceOrder/{serviceOrderId}")]
        public async Task<IActionResult> GetByServiceOrder(int serviceOrderId)
        {
            var comments = await _commentService.GetCommentsByServiceOrderIdAsync(serviceOrderId);
            return Ok(comments);
        }

        // GET: api/comment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            return comment == null ? NotFound() : Ok(comment);
        }

        // POST: api/comment
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Zakładamy, że identyfikator autora pochodzi z tokena JWT
            var authorId = User.Identity?.Name;
            if (string.IsNullOrEmpty(authorId))
                return Unauthorized();

            var comment = await _commentService.CreateCommentAsync(dto, authorId);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }

        // PUT: api/comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto dto)
        {
            var updated = await _commentService.UpdateCommentAsync(id, dto);
            return updated ? Ok() : NotFound();
        }

        // DELETE: api/comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _commentService.DeleteCommentAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
