using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;

namespace ProjektZaliczeniowyNET.Mappers
{
    public interface IServiceOrderMapper
    {
        ServiceOrderDto ToDto(ServiceOrder order);
        ServiceOrderListDto ToListDto(ServiceOrder order);
        ServiceOrderSelectDto ToSelectDto(ServiceOrder order);
        ServiceOrder ToEntity(ServiceOrderCreateDto dto, string createdByUserId);
        void UpdateEntity(ServiceOrder order, ServiceOrderUpdateDto dto);
    }
}