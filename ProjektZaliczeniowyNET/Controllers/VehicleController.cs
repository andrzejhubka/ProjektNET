using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Services;
using System.Threading.Tasks;

namespace ProjektZaliczeniowyNET.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

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
                IsActive = vehicle.IsActive
            };

            return View(dto);
        }

        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVehicleDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

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
    }
}
