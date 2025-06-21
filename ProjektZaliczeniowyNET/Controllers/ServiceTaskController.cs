using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.Interfaces;

namespace ProjektZaliczeniowyNET.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceTaskController : ControllerBase
{
    private readonly IServiceTaskService _service;

    public ServiceTaskController(IServiceTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _service.GetAllAsync();
        return Ok(tasks);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceTaskCreateDto dto)
    {
        try
        {
            var task = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceTaskUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> MarkAsCompleted(int id)
    {
        var success = await _service.MarkAsCompletedAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPatch("{id}/uncomplete")]
    public async Task<IActionResult> MarkAsNotCompleted(int id)
    {
        var success = await _service.MarkAsNotCompletedAsync(id);
        return success ? NoContent() : NotFound();
    }
}
