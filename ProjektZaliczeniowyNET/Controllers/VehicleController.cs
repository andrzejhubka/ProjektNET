using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Services;
using System.Threading.Tasks;

namespace ProjektZaliczeniowyNET.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ICustomerService _customerService;

        public VehicleController(IVehicleService vehicleService, ICustomerService customerService)
        {
            _vehicleService = vehicleService;
            _customerService = customerService;
        }

        // GET: Vehicle
        [HttpGet("/Vehicle")]
        [HttpGet("/Vehicle/Index")]
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return View(vehicles);
        }

        // GET: Vehicle/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        // GET: Vehicle/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCustomersToViewBag();
            return View();
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCustomersToViewBag();
                return View(dto);
            }

            await _vehicleService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicle/Edit/5
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

        // POST: Vehicle/Edit/5
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

        // POST: Vehicle/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _vehicleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }

        // Private helper to populate ViewBag
        private async Task LoadCustomersToViewBag(int? selectedId = null)
        {
            var customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Customers = new SelectList(customers, "Id", "FullName", selectedId);
        }
    }
}
