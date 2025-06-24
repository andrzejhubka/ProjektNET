using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.Part;
using ProjektZaliczeniowyNET.Services;
using System.Threading.Tasks;

namespace ProjektZaliczeniowyNET.Controllers
{
    [Authorize(Roles = "Admin,Mechanik,Recepcjonista")]
    public class PartController : Controller
    {
        private readonly IPartService _partService;

        public PartController(IPartService partService)
        {
            _partService = partService;
        }

        // GET: Part
        [HttpGet("/Part")]
        [HttpGet("/Part/Index")]
        public async Task<IActionResult> Index()
        {
            var parts = await _partService.GetAllAsync();
            return View(parts);
        }

        // GET: Part/Create
        [HttpGet]
        public IActionResult Create(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Part/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartCreateDto dto, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _partService.CreateAsync(dto);
            
            // Sprawdź czy jest returnUrl i przekieruj tam
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: /Part/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var part = await _partService.GetByIdAsync(id);
            if (part == null)
                return NotFound();

            var dto = new PartUpdateDto
            {
                Name = part.Name,
                Description = part.Description,
                UnitPrice = part.UnitPrice,
                QuantityInStock = part.QuantityInStock
            };

            return View(dto);
        }

        // POST: /Part/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PartUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var updated = await _partService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Part/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var part = await _partService.GetByIdAsync(id);
            if (part == null)
                return NotFound();

            return View(part); // PartDto
        }

        // POST: /Part/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _partService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return Ok(); // dla JS
        }
    }
}