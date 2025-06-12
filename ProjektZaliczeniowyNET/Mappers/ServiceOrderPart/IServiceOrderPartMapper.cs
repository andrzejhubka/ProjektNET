using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;

namespace ProjektZaliczeniowyNET.Mappers
{
    public interface IServiceOrderPartMapper
    {
        ServiceOrderPartDto ToDto(ServiceOrderPart entity);
        ServiceOrderPartListDto ToListDto(ServiceOrderPart entity);
        ServiceOrderPart ToEntity(ServiceOrderPartCreateDto dto);
        void UpdateEntity(ServiceOrderPart entity, ServiceOrderPartUpdateDto dto);
    }
}