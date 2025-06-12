using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;

namespace ProjektZaliczeniowyNET.Mappers
{
    public class ServiceTaskMapper : IServiceTaskMapper
    {
        public ServiceTaskDto ToDto(ServiceTask serviceTask)
        {
            if (serviceTask == null)
                throw new ArgumentNullException(nameof(serviceTask));

            return new ServiceTaskDto
            {
                Id = serviceTask.Id,
                Description = serviceTask.Description,
                DetailedDescription = serviceTask.DetailedDescription,
                LaborHours = serviceTask.LaborHours,
                HourlyRate = serviceTask.HourlyRate,
                LaborCost = serviceTask.LaborCost,
                TotalTaskCost = serviceTask.TotalTaskCost,
                IsCompleted = serviceTask.IsCompleted,
                CompletedAt = serviceTask.CompletedAt,
                Notes = serviceTask.Notes,
                ServiceOrderId = serviceTask.ServiceOrderId,
                ServiceOrderNumber = serviceTask.ServiceOrder?.OrderNumber ?? string.Empty
            };
        }

        public ServiceTaskListDto ToListDto(ServiceTask serviceTask)
        {
            if (serviceTask == null)
                throw new ArgumentNullException(nameof(serviceTask));

            return new ServiceTaskListDto
            {
                Id = serviceTask.Id,
                Description = serviceTask.Description,
                LaborHours = serviceTask.LaborHours,
                HourlyRate = serviceTask.HourlyRate,
                LaborCost = serviceTask.LaborCost,
                IsCompleted = serviceTask.IsCompleted,
                CompletedAt = serviceTask.CompletedAt,
                ServiceOrderId = serviceTask.ServiceOrderId,
                ServiceOrderNumber = serviceTask.ServiceOrder?.OrderNumber ?? string.Empty
            };
        }

        public ServiceTask ToEntity(ServiceTaskCreateDto createDto)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            return new ServiceTask
            {
                Description = createDto.Description,
                DetailedDescription = createDto.DetailedDescription,
                LaborHours = createDto.LaborHours,
                HourlyRate = createDto.HourlyRate,
                LaborCost = createDto.LaborHours * createDto.HourlyRate,
                Notes = createDto.Notes,
                ServiceOrderId = createDto.ServiceOrderId,
                IsCompleted = false,
                CompletedAt = null
            };
        }

        public void UpdateEntity(ServiceTask serviceTask, ServiceTaskUpdateDto updateDto)
        {
            if (serviceTask == null)
                throw new ArgumentNullException(nameof(serviceTask));
            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));

            serviceTask.Description = updateDto.Description;
            serviceTask.DetailedDescription = updateDto.DetailedDescription;
            serviceTask.LaborHours = updateDto.LaborHours;
            serviceTask.HourlyRate = updateDto.HourlyRate;
            serviceTask.LaborCost = updateDto.LaborHours * updateDto.HourlyRate;
            serviceTask.Notes = updateDto.Notes;
            
            // Obsługa zmiany statusu ukończenia
            if (updateDto.IsCompleted && !serviceTask.IsCompleted)
            {
                serviceTask.IsCompleted = true;
                serviceTask.CompletedAt = DateTime.UtcNow;
            }
            else if (!updateDto.IsCompleted && serviceTask.IsCompleted)
            {
                serviceTask.IsCompleted = false;
                serviceTask.CompletedAt = null;
            }
        }

        public IEnumerable<ServiceTaskDto> ToDtoList(IEnumerable<ServiceTask> serviceTasks)
        {
            if (serviceTasks == null)
                return Enumerable.Empty<ServiceTaskDto>();

            return serviceTasks.Select(ToDto);
        }

        public IEnumerable<ServiceTaskListDto> ToListDtoList(IEnumerable<ServiceTask> serviceTasks)
        {
            if (serviceTasks == null)
                return Enumerable.Empty<ServiceTaskListDto>();

            return serviceTasks.Select(ToListDto);
        }
    }
}