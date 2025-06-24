using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.DTOs.Comment;
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
    private readonly ICommentService _commentService;

    public ServiceOrderController(
        IServiceOrderService serviceOrderService,
        ICustomerService customerService,
        IVehicleService vehicleService,
        ServiceOrderMapper mapper,
        UserManager<ApplicationUser> userManager,
        IPartService partService,
        ICommentService commentService
    )
    {
        _serviceOrderService = serviceOrderService;
        _customerService = customerService;
        _vehicleService = vehicleService;
        _mapper = mapper;  
        _userManager = userManager;
        _partService = partService;
        _commentService = commentService;
    }

    [Authorize(Roles = "Admin,Recepcjonista")]
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
    [Authorize(Roles = "Admin,Recepcjonista")]
    public async Task<IActionResult> Create()
    {
        var parts = await _partService.GetAllAsync();

        ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
        ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
        ViewBag.Parts = new SelectList(parts, "Id", "Name");
        ViewBag.PartsData = parts.Select(p => new { p.Id, p.Name, p.UnitPrice }).ToList(); // pełne dane do JS
        ViewBag.Mechanics = new SelectList( await _userManager.GetUsersInRoleAsync("Mechanik"), "Id", "UserName");
        
        // Dodanie opcji komentarzy dla widoku Create
        ViewBag.CommentTypes = new SelectList(Enum.GetValues(typeof(CommentType)).Cast<CommentType>()
            .Select(e => new { Value = (int)e, Text = e.ToString() }), "Value", "Text");
        
        return View(new ServiceOrderCreateDto());
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Recepcjonista")]
    public async Task<IActionResult> Create(ServiceOrderCreateDto dto, string? initialComment, CommentType? commentType)
    {
        if (!ModelState.IsValid)
        {
            var parts = await _partService.GetAllAsync();

            ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
            ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
            ViewBag.Parts = new SelectList(parts, "Id", "Name");
            ViewBag.PartsData = parts.Select(p => new { p.Id, p.Name, p.UnitPrice }).ToList();
            ViewBag.Mechanics = new SelectList( await _userManager.GetUsersInRoleAsync("Mechanik"), "Id", "UserName");
            ViewBag.CommentTypes = new SelectList(Enum.GetValues(typeof(CommentType)).Cast<CommentType>()
                .Select(e => new { Value = (int)e, Text = e.ToString() }), "Value", "Text");

            return View(dto);
        }
    
        var serviceOrder = await _serviceOrderService.CreateAsync(dto);
        
        // Dodaj komentarz początkowy jeśli został podany
        if (!string.IsNullOrWhiteSpace(initialComment) && commentType.HasValue)
        {
            var commentDto = new CommentCreateDto
            {
                Content = initialComment,
                Type = commentType.Value,
                ServiceOrderId = serviceOrder.Id
            };
            
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _commentService.CreateCommentAsync(commentDto, authorId);
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    [Authorize(Roles = "Admin,Recepcjonista")]
    public class UpdateStatusDto
    {
        public int Status { get; set; } // Zmienione na int zamiast ServiceOrderStatus
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Recepcjonista")]
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
    [Authorize(Roles = "Admin,Recepcjonista")]
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
    [Authorize(Roles = "Admin,Recepcjonista")]
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
        
        // Pobierz komentarze dla tego zlecenia
        var comments = await _commentService.GetCommentsByServiceOrderIdAsync(id);
        ViewBag.Comments = comments;
        ViewBag.CommentTypes = new SelectList(Enum.GetValues(typeof(CommentType)).Cast<CommentType>()
            .Select(e => new { Value = (int)e, Text = e.ToString() }), "Value", "Text");
        
        return View(order);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int serviceOrderId, string content, CommentType type)
    {
        if (!string.IsNullOrWhiteSpace(content))
        {
            var commentDto = new CommentCreateDto
            {
                Content = content,
                Type = type,
                ServiceOrderId = serviceOrderId
            };
            
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _commentService.CreateCommentAsync(commentDto, authorId);
        }
        
        return RedirectToAction("Details", new { id = serviceOrderId });
    }
    
    [Authorize(Roles = "Admin,Recepcjonista")]
    public async Task<IActionResult> Delete(int id)
    {
        await _serviceOrderService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> MyOrders(
        int? status,
        string customer,
        string vehicle,
        DateTime? dateFrom,
        DateTime? dateTo,
        string mechanicId)
    {
        // ✅ Pobierz ID aktualnie zalogowanego mechanika
        var currentMechanicId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
        // ✅ Wymuś filtrowanie tylko po tym mechaniku
        var orders = await _serviceOrderService.GetFilteredAsync(
            status, null, null, dateFrom, dateTo, currentMechanicId);

        // Przekaż filtry do ViewBag (bez mechanika - nie potrzebny)
        ViewBag.SelectedStatus = status;
        ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
        ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");
    
        // ✅ Nie przekazuj listy mechaników - mechanik widzi tylko swoje zlecenia
        // ViewBag.Mechanics = ... // Usuń to
    
        ViewData["Title"] = "Moje zlecenia";
        return View("Index", orders); // Używa tego samego widoku co Index
    }
}