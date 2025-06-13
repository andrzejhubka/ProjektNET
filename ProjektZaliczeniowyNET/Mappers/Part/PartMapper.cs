using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.DTOs.Part;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class PartMapper
    {
        // Usuń 'public' z metod partial
        public partial PartDto ToDto(Part part);
        public partial PartListDto ToListDto(Part part);
        public partial PartSelectDto ToSelectDto(Part part);
        public partial Part ToEntity(PartCreateDto dto);
        public partial void UpdateEntity([MappingTarget] Part part, PartUpdateDto dto);

    }
}