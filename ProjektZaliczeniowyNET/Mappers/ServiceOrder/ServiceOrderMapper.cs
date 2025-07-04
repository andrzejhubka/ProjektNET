using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrder;

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class ServiceOrderMapper
    {
        public partial ServiceOrder ToEntity(ServiceOrderCreateDto dto);

        public partial ServiceOrderDto ToDto(ServiceOrder entity);

        public partial ServiceOrderListDto ToListDto(ServiceOrder entity);
        
        public partial void UpdateEntity(ServiceOrder entity, ServiceOrderUpdateDto dto);
        
        public partial ServiceOrderUpdateDto ToUpdateDto(ServiceOrderDto dto);
    }
}