using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
        ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
        ViewBag.Parts = new SelectList(await _partService.GetAllAsync(), "Id", "Name");
        ViewBag.Mechanics = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");
        
        return View(new ServiceOrderCreateDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ServiceOrderCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customers = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "FullName");
            ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "DisplayName");
            ViewBag.Parts = new SelectList(await _partService.GetAllAsync(), "Id", "Name");
            ViewBag.Mechanics = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");

            return View(dto);
        }
    
        await _serviceOrderService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }
}
