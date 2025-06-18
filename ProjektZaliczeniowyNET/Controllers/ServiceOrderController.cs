using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;

namespace ProjektZaliczeniowyNET.Controllers;

[Controller]
public class ServiceOrderController : Controller
{
    private readonly IServiceOrderService _serviceOrderService;
    private readonly ICustomerService _customerService;
    private readonly IVehicleService _vehicleService;

    public ServiceOrderController(
        IServiceOrderService serviceOrderService,
        ICustomerService customerService,
        IVehicleService vehicleService)
    {
        _serviceOrderService = serviceOrderService;
        _customerService = customerService;
        _vehicleService = vehicleService;
    }

    public async Task<IActionResult> Index()
    {
        var serviceOrders = await _serviceOrderService.GetAllAsync();
        return View(serviceOrders);
    }
    
    // GET: /ServiceOrder
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _serviceOrderService.GetAllAsync();
        return Ok(orders);
    }

    // GET: /ServiceOrder/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _serviceOrderService.GetByIdAsync(id);
        return order == null ? NotFound() : Ok(order);
    }

    // POST: /ServiceOrder
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceOrderCreateDto dto)
    {
        // Przykład: użytkownik twórca — zakładamy autoryzację i ID użytkownika
        var userId = User?.Identity?.Name ?? "system"; // zamień na pobieranie z tokena, jeśli masz JWT

        var created = await _serviceOrderService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: /ServiceOrder/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceOrderUpdateDto dto)
    {
        var updated = await _serviceOrderService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: /ServiceOrder/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _serviceOrderService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // GET: /ServiceOrder/active/count
    [HttpGet("active/count")]
    public async Task<IActionResult> GetActiveOrdersCount()
    {
        var count = await _serviceOrderService.GetActiveOrdersCountAsync();
        return Ok(new { count });
    }
    
    [HttpPut("{id}/assign-mechanic")]
    public async Task<IActionResult> AssignMechanic(int id, [FromBody] string mechanicId)
    {
        var updateDto = new ServiceOrderUpdateDto
        {
            AssignedMechanicId = mechanicId
        };

        var result = await _serviceOrderService.UpdateAsync(id, updateDto);
        return result ? NoContent() : NotFound();
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Customers = await _customerService.GetAllCustomersAsync();
        ViewBag.Vehicles = await _vehicleService.GetAllAsync();

        return View(new ServiceOrderCreateDto());
    }
}
