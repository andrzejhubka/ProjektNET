using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.Services;

namespace ProjektZaliczeniowyNET.Controllers
{
    [Authorize(Roles = "Admin,Recepcjonista")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customer
        [HttpGet("/Customer")]
        [HttpGet("/Customer/Index")]
        public async Task<IActionResult> Index(string? search)
        {
            var customers = await _customerService.GetAllCustomersAsync(search);
            return View(customers);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // GET: Customer/Create
        [HttpGet]
        public IActionResult Create(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateDto dto, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(dto);

            if (!await _customerService.IsEmailUniqueAsync(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Podany email już istnieje.");
                return View(dto);
            }

            await _customerService.CreateCustomerAsync(dto);
            
            // Sprawdź czy jest returnUrl i przekieruj tam
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();

            // Mapowanie do CustomerUpdateDto jeśli masz osobny DTO do edycji
            var dto = new CustomerUpdateDto
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Notes = customer.Notes
            };
            return View(dto);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            if (!await _customerService.IsEmailUniqueAsync(dto.Email, id))
            {
                ModelState.AddModelError(nameof(dto.Email), "Podany email już istnieje.");
                return View(dto);
            }

            var result = await _customerService.UpdateCustomerAsync(id, dto);
            if (result == null) return NotFound();

            return RedirectToAction(nameof(Index));
        }
        
        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
