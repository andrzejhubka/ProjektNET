using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using ProjektZaliczeniowyNET.Interfaces;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Services
{
    public class ServiceTaskService : IServiceTaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceTaskMapper _mapper;
        private readonly ILogger<ServiceTaskService> _logger;

        public ServiceTaskService(
            ApplicationDbContext context,
            ServiceTaskMapper mapper,
            ILogger<ServiceTaskService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ServiceTaskListDto>> GetAllAsync()
        {
            var serviceTasks = await _context.ServiceTasks
                .OrderBy(st => st.Id)
                .ToListAsync();

            return _mapper.ToListDtoList(serviceTasks);
        }
        
        public async Task<ServiceTaskDto?> GetByIdAsync(int id)
        {
            var serviceTask = await _context.ServiceTasks
                .FirstOrDefaultAsync(st => st.Id == id);

            return serviceTask == null ? null : _mapper.ToDto(serviceTask);
        }

        public async Task<ServiceTaskDto> CreateAsync(ServiceTaskCreateDto createDto)
        {
            var serviceTask = _mapper.ToEntity(createDto);
    
            _context.ServiceTasks.Add(serviceTask);
            await _context.SaveChangesAsync();
    
            return _mapper.ToDto(serviceTask);
        }

        public async Task<ServiceTaskDto?> UpdateAsync(int id, ServiceTaskUpdateDto updateDto)
        {
            var serviceTask = await _context.ServiceTasks
                .FirstOrDefaultAsync(st => st.Id == id);

            if (serviceTask == null)
                return null;

            _mapper.UpdateEntity(serviceTask, updateDto);
            await _context.SaveChangesAsync();

            return _mapper.ToDto(serviceTask);
        }
        
        public async Task UpdateManyAsync(int serviceOrderId, List<ServiceTaskCreateDto> newTasks)
        {
            // Usuń wszystkie istniejące
            var existingTasks = await _context.ServiceTasks
                .Where(t => t.ServiceOrderId == serviceOrderId)
                .ToListAsync();
    
            _context.ServiceTasks.RemoveRange(existingTasks);

            // Dodaj nowe
            if (newTasks != null && newTasks.Any())
            {
                foreach (var taskDto in newTasks)
                {
                    var task = _mapper.ToEntity(taskDto);
                    task.ServiceOrderId = serviceOrderId;
                    _context.ServiceTasks.Add(task);
                }
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteManyAsync(int serviceOrderId, List<ServiceTask> newTasks)
        {
            // Usuń wszystkie istniejące
            var existingTasks = await _context.ServiceTasks
                .Where(t => t.ServiceOrderId == serviceOrderId)
                .ToListAsync();
    
            _context.ServiceTasks.RemoveRange(existingTasks);
            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var serviceTask = await _context.ServiceTasks
                .FirstOrDefaultAsync(st => st.Id == id);

            if (serviceTask == null)
                return false;

            _context.ServiceTasks.Remove(serviceTask);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> MarkAsCompletedAsync(int id)
        {
            var serviceTask = await _context.ServiceTasks
                .FirstOrDefaultAsync(st => st.Id == id);

            if (serviceTask == null)
                return false;

            serviceTask.IsCompleted = true;

            await _context.SaveChangesAsync();
    
            return true;
        }
        
        public async Task<bool> MarkAsNotCompletedAsync(int id)
        {
            var serviceTask = await _context.ServiceTasks
                .FirstOrDefaultAsync(st => st.Id == id);

            if (serviceTask == null)
                return false;

            serviceTask.IsCompleted = false;

            await _context.SaveChangesAsync();
    
            return true;
        }
    }
}