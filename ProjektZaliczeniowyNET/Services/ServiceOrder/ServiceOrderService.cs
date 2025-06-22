using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.Mappers;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Data;

namespace ProjektZaliczeniowyNET.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceOrderMapper _mapper;

        public ServiceOrderService(ApplicationDbContext context, ServiceOrderMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceOrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.ServiceOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.AssignedMechanic)
                .Include(o => o.ServiceTasks)
                .Include(o => o.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return null;

            return _mapper.ToDto(order);
        }

        public async Task<IEnumerable<ServiceOrderListDto>> GetAllAsync()
        {
            var orders = await _context.ServiceOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.AssignedMechanic)
                .Include(o => o.ServiceTasks)
                .ToListAsync();

            return orders.Select(o => _mapper.ToListDto(o));
        }

        public async Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto)
        {
            var order = _mapper.ToEntity(dto);
            _context.ServiceOrders.Add(order);
            await _context.SaveChangesAsync();
            return _mapper.ToDto(order);
        }

        public async Task<bool> UpdateAsync(int id, ServiceOrderUpdateDto dto)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return false;

            _mapper.UpdateEntity(order, dto);
            _context.ServiceOrders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return false;

            _context.ServiceOrders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetActiveOrdersCountAsync()
        {
            return await _context.ServiceOrders
                .CountAsync(o => o.Status == ServiceOrderStatus.InProgress || o.Status == ServiceOrderStatus.Pending);
        }

        // Poprawiona metoda do aktualizacji statusu
        public async Task<bool> UpdateStatusAsync(int id, ServiceOrderStatus newStatus)
        {
            try
            {
                var order = await _context.ServiceOrders.FindAsync(id);
                if (order == null) 
                    return false;

                order.Status = newStatus;
                
                // Dodaj śledzenie zmian dla debugowania
                _context.Entry(order).State = EntityState.Modified;
                
                var result = await _context.SaveChangesAsync();
                return result > 0; // Sprawdź czy faktycznie zapisano zmiany
            }
            catch (Exception ex)
            {
                // Logowanie błędu - możesz użyć swojego systemu logowania
                // _logger.LogError(ex, "Błąd podczas aktualizacji statusu zlecenia {OrderId}", id);
                throw; // Rzuć wyjątek dalej, żeby kontroler mógł go obsłużyć
            }
        }
    }
}