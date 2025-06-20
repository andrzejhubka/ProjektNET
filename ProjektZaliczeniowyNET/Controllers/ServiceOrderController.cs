using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.Mappers;

namespace ProjektZaliczeniowyNET.Controllers;

[Controller]
public class ServiceOrderController : Controller
{
    private readonly IServiceOrderService _serviceOrderService;
    private readonly ICustomerService _customerService;
    private readonly IVehicleService _vehicleService;
    private readonly ServiceOrderMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPartService _partService;

    public ServiceOrderController(
        IServiceOrderService serviceOrderService,
        ICustomerService customerService,
        IVehicleService vehicleService,
        ServiceOrderMapper mapper,
        UserManager<IdentityUser> userManager,
        IPartService partService
    )
    {
        _serviceOrderService = serviceOrderService;
        _customerService = customerService;
        _vehicleService = vehicleService;
        _mapper = mapper;  
        _userManager = userManager;
        _partService = partService;
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

    [HttpPost]
    public async Task<IActionResult> Create(ServiceOrderCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customers = (await _customerService.GetAllCustomersAsync())
                .Select(c => new SelectListItem(c.FullName, c.Id.ToString())).ToList();

            ViewBag.Vehicles = (await _vehicleService.GetAllAsync())
                .Select(v => new SelectListItem(v.DisplayName, v.Id.ToString())).ToList();

            return View(dto);
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

        var createdDto = await _serviceOrderService.CreateAsync(dto, userId);

        return RedirectToAction(nameof(Index));
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
        ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
        ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
        ViewBag.Parts = new SelectList(await _partService.GetAllAsync(), "Id", "Name");
        ViewBag.Mechanics = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");
        
        // Debug części
        var parts = await _partService.GetAllAsync();
        Console.WriteLine($"Liczba części z serwisu: {parts.Count()}");
        ViewBag.Parts = new SelectList(parts, "Id", "Name");
        return View(new ServiceOrderCreateDto());
    }
}
