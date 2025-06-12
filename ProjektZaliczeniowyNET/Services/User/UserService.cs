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
        private readonly IUserMapper _userMapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            IUserMapper userMapper,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userDto = _userMapper.ToUserDto(user, roles);
                    userDtos.Add(userDto);
                }

                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return null;

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _userMapper.ToUserDto(user, roles);

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user by id: {userId}");
                throw;
            }
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return null;

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _userMapper.ToUserDto(user, roles);

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user by email: {email}");
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(UserCreateDto createUserDto)
        {
            try
            {
                var user = _userMapper.ToUser(createUserDto);
                user.CreatedAt = DateTime.UtcNow;
                user.IsActive = true;

                var result = await _userManager.CreateAsync(user, createUserDto.Password);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to create user: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    return false;
                }

                if (createUserDto.Roles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, createUserDto.Roles);
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to add roles to user: {Errors}", 
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(string userId, UserUpdateDto updateUserDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) 
                {
                    _logger.LogWarning("User with id {UserId} not found", userId);
                    return false;
                }

                _userMapper.UpdateUserFromDto(updateUserDto, user);
                
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to update user: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    return false;
                }

                // Aktualizacja ról
                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesToRemove = currentRoles.Except(updateUserDto.Roles).ToList();
                var rolesToAdd = updateUserDto.Roles.Except(currentRoles).ToList();

                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to remove roles: {Errors}", 
                            string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                    }
                }

                if (rolesToAdd.Any())
                {
                    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to add roles: {Errors}", 
                            string.Join(", ", addResult.Errors.Select(e => e.Description)));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) 
                {
                    _logger.LogWarning("User with id {UserId} not found", userId);
                    return false;
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to delete user: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> ToggleUserStatusAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User with id {UserId} not found", userId);
                    return false;
                }

                user.IsActive = !user.IsActive;
                var result = await _userManager.UpdateAsync(user);

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling user status: {UserId}", userId);
                return false;
            }
        }

        public async Task<IEnumerable<UserListDto>> GetUsersForListAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userListDtos = new List<UserListDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userListDto = _userMapper.ToUserListDto(user, roles);
                    userListDtos.Add(userListDto);
                }

                return userListDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users for list");
                throw;
            }
        }
    }
}