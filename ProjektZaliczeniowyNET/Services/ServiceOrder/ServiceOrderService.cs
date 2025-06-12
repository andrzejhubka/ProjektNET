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
        private readonly IServiceOrderMapper _mapper;

        public ServiceOrderService(ApplicationDbContext context, IServiceOrderMapper mapper)
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
                .Include(o => o.CreatedByUser)
                .Include(o => o.ServiceTasks)
                .Include(o => o.Comments)
                    .ThenInclude(c => c.Author)
                .Include(o => o.ServiceOrderParts)
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

        public async Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto, string createdByUserId)
        {
            var order = _mapper.ToEntity(dto, createdByUserId);
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
    }
}
