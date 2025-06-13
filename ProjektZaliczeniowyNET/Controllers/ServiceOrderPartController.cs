using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;
using ProjektZaliczeniowyNET.Services;

namespace ProjektZaliczeniowyNET.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceOrderPartController : ControllerBase
{
    private readonly IServiceOrderPartService _service;

    public ServiceOrderPartController(IServiceOrderPartService service)
    {
        _service = service;
    }

    // GET: api/ServiceOrderPart/byServiceOrder/5
    [HttpGet("byServiceOrder/{serviceOrderId}")]
    public async Task<IActionResult> GetAllByServiceOrderId(int serviceOrderId)
    {
        var parts = await _service.GetAllByServiceOrderIdAsync(serviceOrderId);
        return Ok(parts);
    }

    // GET: api/ServiceOrderPart/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var part = await _service.GetByIdAsync(id);
        return part == null ? NotFound() : Ok(part);
    }

    // POST: api/ServiceOrderPart
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceOrderPartCreateDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // PUT: api/ServiceOrderPart/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceOrderPartUpdateDto dto)
    {
        try
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // DELETE: api/ServiceOrderPart/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
