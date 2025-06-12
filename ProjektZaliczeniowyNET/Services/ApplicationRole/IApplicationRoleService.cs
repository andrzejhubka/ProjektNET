using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.DTOs.Role;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Services
{
    public interface IApplicationRoleService
    {
        Task<IEnumerable<ApplicationRoleListDto>> GetAllRolesAsync();
        Task<IEnumerable<ApplicationRoleSelectDto>> GetRolesForSelectAsync();
        Task<ApplicationRoleListDto?> GetRoleByIdAsync(string id);
        Task<ApplicationRoleListDto?> GetRoleByNameAsync(string name);
        Task<IdentityResult> CreateRoleAsync(string name, string? description = null);
        Task<IdentityResult> UpdateRoleAsync(string id, string name, string? description = null);
        Task<IdentityResult> DeleteRoleAsync(string id);
        Task<bool> RoleExistsAsync(string name);
        Task InitializeDefaultRolesAsync();
    }
}