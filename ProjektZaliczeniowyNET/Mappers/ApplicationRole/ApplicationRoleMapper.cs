using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.DTOs.Role;
using ProjektZaliczeniowyNET.Models; // Załóżmy, że masz klasę ApplicationRole w Models

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class ApplicationRoleMapper
    {
        // Mapowanie do listy (szczegóły)
        public partial ApplicationRoleListDto ToListDto(ApplicationRole role);

        // Mapowanie do select (prostsza wersja)
        public partial ApplicationRoleSelectDto ToSelectDto(ApplicationRole role);
    }
}