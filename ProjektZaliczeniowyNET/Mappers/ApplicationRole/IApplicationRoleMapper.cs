using ProjektZaliczeniowyNET.DTOs.Role;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers
{
    public interface IApplicationRoleMapper
    {
        ApplicationRoleListDto ToListDto(ApplicationRole role);
        ApplicationRoleSelectDto ToSelectDto(ApplicationRole role);
        IEnumerable<ApplicationRoleListDto> ToListDto(IEnumerable<ApplicationRole> roles);
        IEnumerable<ApplicationRoleSelectDto> ToSelectDto(IEnumerable<ApplicationRole> roles);
        ApplicationRole ToEntity(ApplicationRoleListDto dto);
    }
}