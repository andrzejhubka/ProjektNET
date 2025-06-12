using ProjektZaliczeniowyNET.DTOs.Role;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers
{
    public class ApplicationRoleMapper : IApplicationRoleMapper
    {
        public ApplicationRoleListDto ToListDto(ApplicationRole role)
        {
            return new ApplicationRoleListDto
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description,
                CreatedAt = role.CreatedAt
            };
        }

        public ApplicationRoleSelectDto ToSelectDto(ApplicationRole role)
        {
            return new ApplicationRoleSelectDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public IEnumerable<ApplicationRoleListDto> ToListDto(IEnumerable<ApplicationRole> roles)
        {
            return roles.Select(ToListDto);
        }

        public IEnumerable<ApplicationRoleSelectDto> ToSelectDto(IEnumerable<ApplicationRole> roles)
        {
            return roles.Select(ToSelectDto);
        }

        public ApplicationRole ToEntity(ApplicationRoleListDto dto)
        {
            return new ApplicationRole(dto.Name)
            {
                Id = dto.Id,
                Description = dto.Description,
                CreatedAt = dto.CreatedAt
            };
        }
    }
}