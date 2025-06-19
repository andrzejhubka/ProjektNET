using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.Part;
using ProjektZaliczeniowyNET.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Recepcjonista")]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartsController(IPartService partService)
        {
            _partService = partService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartListDto>>> GetAll()
        {
            var parts = await _partService.GetAllAsync();
            return Ok(parts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PartDto>> GetById(int id)
        {
            var part = await _partService.GetByIdAsync(id);
            if (part == null) return NotFound();
            return Ok(part);
        }

        [HttpPost]
        public async Task<ActionResult<PartDto>> Create(PartCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdPart = await _partService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdPart.Id }, createdPart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PartUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _partService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _partService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
