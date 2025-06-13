using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class ServiceOrderPartMapper
    {
        // Mapowanie z DTO tworzenia na encję
        public partial ServiceOrderPart ToEntity(ServiceOrderPartCreateDto dto);

        // Mapowanie z encji na DTO szczegółowe
        public partial ServiceOrderPartDto ToDto(ServiceOrderPart entity);

        // Mapowanie z encji na DTO listowe
        public partial ServiceOrderPartListDto ToListDto(ServiceOrderPart entity);

        // Aktualizacja encji na podstawie DTO aktualizacji
        public partial void UpdateEntity(ServiceOrderPart entity, ServiceOrderPartUpdateDto dto);
    }
}