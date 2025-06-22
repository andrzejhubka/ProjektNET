using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.Mappers;
using System.Threading.Tasks;
using ProjektZaliczeniowyNET.Models;

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
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var parts = await _partService.GetAllAsync();

        ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
        ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
        ViewBag.Parts = new SelectList(parts, "Id", "Name");
        ViewBag.PartsData = parts.Select(p => new { p.Id, p.Name, p.UnitPrice }).ToList(); // pełne dane do JS
        ViewBag.Mechanics = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");
        
        return View(new ServiceOrderCreateDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ServiceOrderCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var parts = await _partService.GetAllAsync();

            ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
            ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
            ViewBag.Parts = new SelectList(parts, "Id", "Name");
            ViewBag.PartsData = parts.Select(p => new { p.Id, p.Name, p.UnitPrice }).ToList();
            ViewBag.Mechanics = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");

            return View(dto);
        }
    
        await _serviceOrderService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    // DTO dla UpdateStatus - przeniesione poza kontroler
    public class UpdateStatusDto
    {
        public int Status { get; set; } // Zmienione na int zamiast ServiceOrderStatus
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        try
        {
            if (dto == null)
                return BadRequest("Brak danych.");

            // Sprawdź czy wartość statusu jest prawidłowa
            if (!Enum.IsDefined(typeof(ServiceOrderStatus), dto.Status))
                return BadRequest("Nieprawidłowa wartość statusu.");

            var status = (ServiceOrderStatus)dto.Status;
            var success = await _serviceOrderService.UpdateStatusAsync(id, status);
            
            if (!success) 
                return NotFound("Nie znaleziono zlecenia o podanym ID.");

            return Ok(new { message = "Status został pomyślnie zaktualizowany" });
        }
        catch (Exception ex)
        {
            // Logowanie błędu
            return StatusCode(500, $"Błąd serwera: {ex.Message}");
        }
    }
}