using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Services;
using System.Threading.Tasks;

namespace ProjektZaliczeniowyNET.Controllers
{
    [Authorize(Roles = "Admin,Mechanik,Recepcjonista")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ICustomerService _customerService;

        public VehicleController(IVehicleService vehicleService, ICustomerService customerService)
        {
            _vehicleService = vehicleService;
            _customerService = customerService;
        }

        [HttpGet("/Vehicle")]
        [HttpGet("/Vehicle/Index")]
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return View(vehicles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCustomersToViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCreateDto dto, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                await LoadCustomersToViewBag(dto.CustomerId);
                return View(dto);
            }

            await _vehicleService.CreateAsync(dto);
            
            // Sprawdź czy jest returnUrl i przekieruj tam
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();

            var dto = new UpdateVehicleDto
            {
                VIN = vehicle.VIN,
                LicensePlate = vehicle.LicensePlate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color,
                EngineNumber = vehicle.EngineNumber,
                Mileage = vehicle.Mileage,
                FuelType = vehicle.FuelType,
                Notes = vehicle.Notes,
                ImageUrl = vehicle.ImageUrl,
                IsActive = vehicle.IsActive,
                CustomerId = vehicle.CustomerId
            };

            await LoadCustomersToViewBag(vehicle.CustomerId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVehicleDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCustomersToViewBag(dto.CustomerId);
                return View(dto);
            }

            var updated = await _vehicleService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _vehicleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }

        private async Task LoadCustomersToViewBag(int? selectedId = null)
        {
            var customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Customers = new SelectList(customers, "Id", "FullName", selectedId);
        }
    }
}
