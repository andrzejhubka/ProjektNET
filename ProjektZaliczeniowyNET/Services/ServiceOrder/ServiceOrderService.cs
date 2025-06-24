using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;
using ProjektZaliczeniowyNET.Mappers;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.Interfaces;

namespace ProjektZaliczeniowyNET.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceOrderMapper _mapper;
        private readonly IServiceTaskService _taskService;

        public ServiceOrderService(ApplicationDbContext context, ServiceOrderMapper mapper,  IServiceTaskService taskService)
        {
            _context = context;
            _mapper = mapper;
            _taskService = taskService;
        }

        public async Task<ServiceOrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.ServiceOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.Mechanic)
                .Include(o => o.ServiceTasks).ThenInclude(s => s.Parts)
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
                .Include(o => o.Mechanic)
                .Include(o => o.ServiceTasks).ThenInclude(s => s.Parts)
                .ToListAsync();

            return orders.Select(o => _mapper.ToListDto(o));
        }
        
        public async Task<IEnumerable<ServiceOrderListDto>> GetFilteredAsync(
            int? status,
            string customer,
            string vehicle,
            DateTime? dateFrom,
            DateTime? dateTo,
            string mechanicId)
        {
            var query = _context.ServiceOrders
                .Include(so => so.Customer)
                .Include(so => so.Vehicle)
                .Include(so => so.Mechanic)
                .AsQueryable();

            // Filtr statusu
            if (status.HasValue)
            {
                query = query.Where(so => (int)so.Status == status.Value);
            }

            // Filtr klienta
            if (!string.IsNullOrEmpty(customer))
            {
                query = query.Where(so => 
                    so.Customer.FirstName.Contains(customer) || 
                    so.Customer.LastName.Contains(customer));
            }

            // Filtr pojazdu
            if (!string.IsNullOrEmpty(vehicle))
            {
                query = query.Where(so => 
                    so.Vehicle.Make.Contains(vehicle) || 
                    so.Vehicle.Model.Contains(vehicle));
            }

            // Filtr daty od
            if (dateFrom.HasValue)
            {
                query = query.Where(so => so.CreatedAt >= dateFrom.Value);
            }

            // Filtr daty do
            if (dateTo.HasValue)
            {
                query = query.Where(so => so.CreatedAt <= dateTo.Value.AddDays(1));
            }

            // Filtr mechanika
            if (!string.IsNullOrEmpty(mechanicId))
            {
                query = query.Where(so => so.AssignedMechanicId == mechanicId);
            }

            var orders = await query
                .OrderByDescending(so => so.CreatedAt)
                .ToListAsync();

            return orders.Select(o => _mapper.ToListDto(o));
        }

        public async Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto)
        {
            var order = _mapper.ToEntity(dto); // Mapuje tylko podstawowe właściwości
            _context.ServiceOrders.Add(order);
            await _context.SaveChangesAsync();
    
            return _mapper.ToDto(order);
        }


        public async Task<bool> UpdateAsync(int id, ServiceOrderUpdateDto dto)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return false;

            var tasksToUpdate = dto.ServiceTasks?.ToList();
            
            // wlasnosci serviceorder
            _mapper.UpdateEntity(order, dto);
            _context.ServiceOrders.Update(order);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"PRZEKAZUJĘ zadania count: {dto.ServiceTasks?.Count ?? 0}");
            // taski z aktualizowanego serviceorder
            await _taskService.UpdateManyAsync(id, tasksToUpdate);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return false;

            // usuwamy wszystkie taski
            var tasksToDelete = order.ServiceTasks;
            await _taskService.DeleteManyAsync(order.Id, tasksToDelete);
            
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