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
        private readonly IServiceTaskMapper _mapper;
        private readonly ILogger<ServiceTaskService> _logger;

        public ServiceTaskService(
            ApplicationDbContext context,
            IServiceTaskMapper mapper,
            ILogger<ServiceTaskService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ServiceTaskListDto>> GetAllAsync()
        {
            try
            {
                var serviceTasks = await _context.ServiceTasks
                    .Include(st => st.ServiceOrder)
                    .OrderBy(st => st.Id)
                    .ToListAsync();

                return _mapper.ToListDtoList(serviceTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania wszystkich zadań serwisowych");
                throw;
            }
        }

        public async Task<IEnumerable<ServiceTaskListDto>> GetByServiceOrderIdAsync(int serviceOrderId)
        {
            try
            {
                var serviceTasks = await _context.ServiceTasks
                    .Include(st => st.ServiceOrder)
                    .Where(st => st.ServiceOrderId == serviceOrderId)
                    .OrderBy(st => st.Id)
                    .ToListAsync();

                return _mapper.ToListDtoList(serviceTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania zadań serwisowych dla zlecenia {ServiceOrderId}", serviceOrderId);
                throw;
            }
        }

        public async Task<ServiceTaskDto?> GetByIdAsync(int id)
        {
            try
            {
                var serviceTask = await _context.ServiceTasks
                    .Include(st => st.ServiceOrder)
                    .FirstOrDefaultAsync(st => st.Id == id);

                return serviceTask == null ? null : _mapper.ToDto(serviceTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania zadania serwisowego o ID {Id}", id);
                throw;
            }
        }

        public async Task<ServiceTaskDto> CreateAsync(ServiceTaskCreateDto createDto)
        {
            try
            {
                // Sprawdź czy zlecenie serwisowe istnieje
                var serviceOrderExists = await _context.ServiceOrders
                    .AnyAsync(so => so.Id == createDto.ServiceOrderId);

                if (!serviceOrderExists)
                {
                    throw new ArgumentException($"Zlecenie serwisowe o ID {createDto.ServiceOrderId} nie istnieje.");
                }

                var serviceTask = _mapper.ToEntity(createDto);
                
                _context.ServiceTasks.Add(serviceTask);
                await _context.SaveChangesAsync();

                // Pobierz utworzone zadanie z danymi nawigacyjnymi
                var createdTask = await _context.ServiceTasks
                    .Include(st => st.ServiceOrder)
                    .FirstAsync(st => st.Id == serviceTask.Id);

                _logger.LogInformation("Utworzono nowe zadanie serwisowe o ID {Id}", serviceTask.Id);
                
                return _mapper.ToDto(createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas tworzenia zadania serwisowego");
                throw;
            }
        }

        public async Task<ServiceTaskDto?> UpdateAsync(int id, ServiceTaskUpdateDto updateDto)
        {
            try
            {
                var serviceTask = await _context.ServiceTasks
                    .FirstOrDefaultAsync(st => st.Id == id);

                if (serviceTask == null)
                {
                    return null;
                }

                _mapper.UpdateEntity(serviceTask, updateDto);
                
                await _context.SaveChangesAsync();

                // Pobierz zaktualizowane zadanie z danymi nawigacyjnymi
                var updatedTask = await _context.ServiceTasks
                    .Include(st => st.ServiceOrder)
                    .FirstAsync(st => st.Id == id);

                _logger.LogInformation("Zaktualizowano zadanie serwisowe o ID {Id}", id);
                
                return _mapper.ToDto(updatedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas aktualizacji zadania serwisowego o ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var serviceTask = await _context.ServiceTasks
                    .FirstOrDefaultAsync(st => st.Id == id);

                if (serviceTask == null)
                {
                    return false;
                }

                _context.ServiceTasks.Remove(serviceTask);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usunięto zadanie serwisowe o ID {Id}", id);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania zadania serwisowego o ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> MarkAsCompletedAsync(int id)
        {
            try
            {
                var serviceTask = await _context.ServiceTasks
                    .FirstOrDefaultAsync(st => st.Id == id);

                if (serviceTask == null)
                {
                    return false;
                }

                serviceTask.IsCompleted = true;
                serviceTask.CompletedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Oznaczono zadanie serwisowe o ID {Id} jako ukończone", id);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas oznaczania zadania serwisowego o ID {Id} jako ukończone", id);
                throw;
            }
        }

        public async Task<bool> MarkAsNotCompletedAsync(int id)
        {
            try
            {
                var serviceTask = await _context.ServiceTasks
                    .FirstOrDefaultAsync(st => st.Id == id);

                if (serviceTask == null)
                {
                    return false;
                }

                serviceTask.IsCompleted = false;
                serviceTask.CompletedAt = null;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Oznaczono zadanie serwisowe o ID {Id} jako nieukończone", id);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas oznaczania zadania serwisowego o ID {Id} jako nieukończone", id);
                throw;
            }
        }

        public async Task<decimal> GetTotalCostByServiceOrderIdAsync(int serviceOrderId)
        {
            try
            {
                var totalCost = await _context.ServiceTasks
                    .Where(st => st.ServiceOrderId == serviceOrderId)
                    .SumAsync(st => st.TotalTaskCost);

                return totalCost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas obliczania całkowitego kosztu zadań dla zlecenia {ServiceOrderId}", serviceOrderId);
                throw;
            }
        }

        public async Task<int> GetCompletedTasksCountByServiceOrderIdAsync(int serviceOrderId)
        {
            try
            {
                var completedCount = await _context.ServiceTasks
                    .Where(st => st.ServiceOrderId == serviceOrderId && st.IsCompleted)
                    .CountAsync();

                return completedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas obliczania liczby ukończonych zadań dla zlecenia {ServiceOrderId}", serviceOrderId);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.ServiceTasks.AnyAsync(st => st.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas sprawdzania istnienia zadania serwisowego o ID {Id}", id);
                throw;
            }
        }
    }
}