using System.Collections.Generic;
using System.Linq;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceTask;
using Riok.Mapperly.Abstractions;

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class ServiceTaskMapper
    {
        public partial ServiceTaskDto ToDto(ServiceTask entity);
        public partial ServiceTaskListDto ToListDto(ServiceTask entity);
        public partial ServiceTask ToEntity(ServiceTaskCreateDto dto);
        public partial void UpdateEntity(ServiceTask entity, ServiceTaskUpdateDto dto);

        // Ręczna metoda mapująca kolekcję
        public IEnumerable<ServiceTaskListDto> ToListDtoList(IEnumerable<ServiceTask> entities)
            => entities.Select(ToListDto);
    }
}