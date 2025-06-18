using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.DTOs.Customer;
using ProjektZaliczeniowyNET.DTOs.Vehicle;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomerMapper _mapper;

        public CustomerService(ApplicationDbContext context, CustomerMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerListDto>> GetAllCustomersAsync(string? search = null)
        {
            var query = _context.Customers
                .Include(c => c.Vehicles)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(c =>
                    c.FirstName.ToLower().Contains(lower) ||
                    c.LastName.ToLower().Contains(lower) ||
                    c.Email.ToLower().Contains(lower) ||
                    c.PhoneNumber.ToLower().Contains(lower));
            }

            var customers = await query
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .ToListAsync();

            return customers.Select(_mapper.ToListDto);
        }

        public async Task<IEnumerable<CustomerSelectDto>> GetCustomersForSelectAsync()
        {
            var customers = await _context.Customers
                .Where(c => c.IsActive)
                .OrderBy(c => c.FullName)
                .ToListAsync();

            return customers.Select(_mapper.ToSelectDto);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                .Include(c => c.ServiceOrders)
                .FirstOrDefaultAsync(c => c.Id == id);

            return customer != null ? _mapper.ToDto(customer) : null;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto createDto)
        {
            var customer = _mapper.ToEntity(createDto);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return _mapper.ToDto(customer);
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerUpdateDto updateDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return null;

            _mapper.UpdateEntity(customer, updateDto);
            await _context.SaveChangesAsync();

            return _mapper.ToDto(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            customer.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            var query = _context.Customers.Where(c => c.Email == email);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<VehicleDto>?> GetVehiclesForCustomerAsync(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                .ThenInclude(v => v.ServiceOrders)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                return null;

            return customer.Vehicles.Select(v => new VehicleDto
            {
                Id = v.Id,
                VIN = v.VIN,
                LicensePlate = v.LicensePlate,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                Color = v.Color,
                EngineNumber = v.EngineNumber,
                Mileage = v.Mileage,
                FuelType = v.FuelType,
                ImageUrl = v.ImageUrl,
                Notes = v.Notes,
                CreatedAt = v.CreatedAt,
                IsActive = v.IsActive,
                DisplayName = $"{v.Make} {v.Model} ({v.LicensePlate})",
                CustomerId = customer.Id,
                CustomerName = customer.FullName,
                CustomerEmail = customer.Email,
                CustomerPhone = customer.PhoneNumber,
                ServiceOrdersCount = v.ServiceOrders?.Count ?? 0
            });
        }
    }
}
