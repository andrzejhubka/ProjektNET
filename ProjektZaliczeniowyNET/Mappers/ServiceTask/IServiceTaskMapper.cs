using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;

namespace ProjektZaliczeniowyNET.Mappers
{
    public interface IServiceTaskMapper
    {
        ServiceTaskDto ToDto(ServiceTask serviceTask);
        ServiceTaskListDto ToListDto(ServiceTask serviceTask);
        ServiceTask ToEntity(ServiceTaskCreateDto createDto);
        void UpdateEntity(ServiceTask serviceTask, ServiceTaskUpdateDto updateDto);
        IEnumerable<ServiceTaskDto> ToDtoList(IEnumerable<ServiceTask> serviceTasks);
        IEnumerable<ServiceTaskListDto> ToListDtoList(IEnumerable<ServiceTask> serviceTasks);
    }
}