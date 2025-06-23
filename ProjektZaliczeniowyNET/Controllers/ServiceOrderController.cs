using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.Mappers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Controllers;

[Authorize(Roles = "Admin,Recepcjonista,Mechanik")]
[Controller]
public class ServiceOrderController : Controller
{
    private readonly IServiceOrderService _serviceOrderService;
    private readonly ICustomerService _customerService;
    private readonly IVehicleService _vehicleService;
    private readonly ServiceOrderMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPartService _partService;

    public ServiceOrderController(
        IServiceOrderService serviceOrderService,
        ICustomerService customerService,
        IVehicleService vehicleService,
        ServiceOrderMapper mapper,
        UserManager<ApplicationUser> userManager,
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

    public async Task<IActionResult> Index(
        int? status,
        string customer,
        string vehicle,
        DateTime? dateFrom,
        DateTime? dateTo,
        string mechanicId)
    {
        var orders = await _serviceOrderService.GetFilteredAsync(
            status, customer, vehicle, dateFrom, dateTo, mechanicId);

        // Przekaż filtry do ViewBag
        ViewBag.SelectedStatus = status;
        ViewBag.CustomerFilter = customer;
        ViewBag.VehicleFilter = vehicle;
        ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
        ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");
        ViewBag.Mechanics = new SelectList( await _userManager.GetUsersInRoleAsync("Mechanik"), "Id", "UserName");
        ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
        ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
        return View(orders);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var parts = await _partService.GetAllAsync();

        ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
        ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
        ViewBag.Parts = new SelectList(parts, "Id", "Name");
        ViewBag.PartsData = parts.Select(p => new { p.Id, p.Name, p.UnitPrice }).ToList(); // pełne dane do JS
        ViewBag.Mechanics = new SelectList( await _userManager.GetUsersInRoleAsync("Mechanik"), "Id", "UserName");
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
            ViewBag.Mechanics = new SelectList( await _userManager.GetUsersInRoleAsync("Mechanik"), "Id", "UserName");

            return View(dto);
        }
    
        await _serviceOrderService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }
    
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
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var order = await _serviceOrderService.GetByIdAsync(id);
        if (order != null)
        {
            Console.WriteLine($"Order ID: {order.Id}"); 
            foreach (var task in order.ServiceTasks)
            {
                Console.WriteLine($"Task : {task.Parts?.Count ?? 0} parts");
            }
            
        }
        else
        {
            Console.WriteLine("ORDER IS NULL - returning NotFound");
            return NotFound();
        }
        
        var orderUpdate = _mapper.ToUpdateDto(order);
     
        // Załaduj listy dropdown
        var parts = await _partService.GetAllAsync();

        ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
        ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
        ViewBag.Parts = new SelectList(parts, "Id", "Name");
        ViewBag.PartsData = parts.Select(p => new { p.Id, p.Name, p.UnitPrice }).ToList();
        ViewBag.Mechanics = new SelectList( await _userManager.GetUsersInRoleAsync("Mechanik"), "Id", "UserName");

        return View(orderUpdate);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServiceOrderUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
            ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
            ViewBag.Mechanics = new SelectList( await _userManager.GetUsersInRoleAsync("Mechanik"), "Id", "UserName");
            ViewBag.Parts = new SelectList(await _partService.GetAllAsync(), "Id", "Name");
            return View(dto);
        }
        
        var result = await _serviceOrderService.UpdateAsync(id, dto);
        
        if (result)
        {
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _serviceOrderService.GetByIdAsync(id);
        if (order != null)
        {
            Console.WriteLine($"Order ID: {order.Id}"); 
            foreach (var task in order.ServiceTasks)
            {
                Console.WriteLine($"Task : {task.Parts?.Count ?? 0} parts");
            }
            
        }
        else
        {
            Console.WriteLine("ORDER IS NULL - returning NotFound");
            return NotFound();
        }
        return View(order);
    }
    
  
    public async Task<IActionResult> Delete(int id)
    {
        await _serviceOrderService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}