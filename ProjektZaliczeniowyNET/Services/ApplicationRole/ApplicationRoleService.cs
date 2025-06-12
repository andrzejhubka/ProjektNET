using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.DTOs.Role;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Services
{
    public class ApplicationRoleService : IApplicationRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IApplicationRoleMapper _mapper;

        public ApplicationRoleService(RoleManager<ApplicationRole> roleManager, IApplicationRoleMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationRoleListDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapper.ToListDto(roles);
        }

        public async Task<IEnumerable<ApplicationRoleSelectDto>> GetRolesForSelectAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapper.ToSelectDto(roles);
        }

        public async Task<ApplicationRoleListDto?> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return role != null ? _mapper.ToListDto(role) : null;
        }

        public async Task<ApplicationRoleListDto?> GetRoleByNameAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            return role != null ? _mapper.ToListDto(role) : null;
        }

        public async Task<IdentityResult> CreateRoleAsync(string name, string? description = null)
        {
            var role = new ApplicationRole(name)
            {
                Description = description
            };

            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(string id, string name, string? description = null)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rola nie została znaleziona." });
            }

            role.Name = name;
            role.NormalizedName = name.ToUpper();
            role.Description = description;

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rola nie została znaleziona." });
            }

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }

        public async Task InitializeDefaultRolesAsync()
        {
            var defaultRoles = new[]
            {
                new { Name = Roles.Admin, Description = "Administrator systemu z pełnymi uprawnieniami" },
                new { Name = Roles.Mechanik, Description = "Mechanik wykonujący naprawy i przeglądy" },
                new { Name = Roles.Recepcjonista, Description = "Recepcjonista obsługujący klientów" }
            };

            foreach (var roleInfo in defaultRoles)
            {
                if (!await RoleExistsAsync(roleInfo.Name))
                {
                    await CreateRoleAsync(roleInfo.Name, roleInfo.Description);
                }
            }
        }
    }
}