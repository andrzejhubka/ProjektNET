using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.DTOs.User;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.Mappers;

namespace ProjektZaliczeniowyNET.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly UserMapper _userMapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            UserMapper userMapper,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserListDto>> GetUsersForListAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserListDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(_userMapper.ToUserListDto(user, roles));
            }

            return result;
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return _userMapper.ToUserDto(user, roles);
        }

        public async Task<bool> CreateUserAsync(UserCreateDto createUserDto)
        {
            var user = _userMapper.ToUser(createUserDto);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            var createResult = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!createResult.Succeeded)
            {
                _logger.LogWarning("Failed to create user: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                return false;
            }

            if (createUserDto.Roles.Any())
            {
                var roleResult = await _userManager.AddToRolesAsync(user, createUserDto.Roles);
                if (!roleResult.Succeeded)
                {
                    _logger.LogWarning("Failed to add roles to user: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            return true;
        }

        public async Task<bool> UpdateUserAsync(string userId, UserUpdateDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return false;
            }

            _userMapper.UpdateUserFromDto(updateUserDto, user);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogWarning("Failed to update user: {Errors}", string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                return false;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = updateUserDto.Roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(updateUserDto.Roles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    _logger.LogWarning("Failed to remove roles: {Errors}", string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                }
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    _logger.LogWarning("Failed to add roles: {Errors}", string.Join(", ", addResult.Errors.Select(e => e.Description)));
                }
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return false;
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                _logger.LogWarning("Failed to delete user: {Errors}", string.Join(", ", deleteResult.Errors.Select(e => e.Description)));
            }

            return deleteResult.Succeeded;
        }

        public async Task<bool> ToggleUserStatusAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                return false;
            }

            user.IsActive = !user.IsActive;
            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded;
        }
    }
}
