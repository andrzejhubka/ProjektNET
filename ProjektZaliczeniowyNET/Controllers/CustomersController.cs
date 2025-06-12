using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.Services;

namespace ProjektZaliczeniowyNET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerListDto>>> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // GET: api/customers/select
        [HttpGet("select")]
        public async Task<ActionResult<IEnumerable<CustomerSelectDto>>> GetForSelect()
        {
            var customers = await _customerService.GetCustomersForSelectAsync();
            return Ok(customers);
        }

        // GET: api/customers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CustomerCreateDto createDto)
        {
            // Można dodatkowo zweryfikować unikalność emaila
            var isUnique = await _customerService.IsEmailUniqueAsync(createDto.Email);
            if (!isUnique)
            {
                ModelState.AddModelError("Email", "Email jest już zajęty.");
                return ValidationProblem(ModelState);
            }

            var createdCustomer = await _customerService.CreateCustomerAsync(createDto);

            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }

        // PUT: api/customers/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CustomerDto>> Update(int id, CustomerUpdateDto updateDto)
        {
            // Sprawdzenie czy email jest unikalny z wykluczeniem aktualnego klienta
            var isUnique = await _customerService.IsEmailUniqueAsync(updateDto.Email, id);
            if (!isUnique)
            {
                ModelState.AddModelError("Email", "Email jest już zajęty.");
                return ValidationProblem(ModelState);
            }

            var updatedCustomer = await _customerService.UpdateCustomerAsync(id, updateDto);
            if (updatedCustomer == null)
                return NotFound();

            return Ok(updatedCustomer);
        }

        // DELETE: api/customers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _customerService.DeleteCustomerAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
