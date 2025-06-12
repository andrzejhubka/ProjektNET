using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.ServiceOrderPart;

namespace ProjektZaliczeniowyNET.Mappers
{
    public class ServiceOrderPartMapper : IServiceOrderPartMapper
    {
        public ServiceOrderPartDto ToDto(ServiceOrderPart entity)
        {
            return new ServiceOrderPartDto
            {
                Id = entity.Id,
                PartName = entity.Part.Name,  // zakładam, że Part ma pole Name
                Cost = entity.Cost,
                Quantity = entity.Quantity,
                ServiceOrderId = entity.ServiceOrderId
            };
        }

        public ServiceOrderPartListDto ToListDto(ServiceOrderPart entity)
        {
            return new ServiceOrderPartListDto
            {
                Id = entity.Id,
                PartName = entity.Part.Name,
                Cost = entity.Cost,
                Quantity = entity.Quantity,
                ServiceOrderId = entity.ServiceOrderId
            };
        }

        public ServiceOrderPart ToEntity(ServiceOrderPartCreateDto dto)
        {
            return new ServiceOrderPart
            {
                ServiceOrderId = dto.ServiceOrderId,
                // PartId nie jest ustawione, bo masz tylko PartName w dto — musisz dopasować PartId po nazwie w serwisie
                Cost = dto.Cost,
                Quantity = dto.Quantity
            };
        }

        public void UpdateEntity(ServiceOrderPart entity, ServiceOrderPartUpdateDto dto)
        {
            // Aktualizacja tylko kosztu i ilości (i nazwy części - patrz komentarz niżej)
            entity.Cost = dto.Cost;
            entity.Quantity = dto.Quantity;
            // Nie aktualizujemy ServiceOrderId i PartId tutaj, ponieważ to mogą wymagać osobnej logiki
        }
    }
}