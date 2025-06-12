using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var userDto = _userMapper.ToUserDto(user);
                    userDto.Roles = await _userManager.GetRolesAsync(user);
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

                var userDto = _userMapper.ToUserDto(user);
                userDto.Roles = await _userManager.GetRolesAsync(user);

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

                var userDto = _userMapper.ToUserDto(user);
                userDto.Roles = await _userManager.GetRolesAsync(user);

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
                var user = _userMapper.ToApplicationUser(createUserDto);
                user.CreatedAt = DateTime.UtcNow;
                user.IsActive = true;

                var result = await _userManager.CreateAsync(user, createUserDto.Password);

                if (!result.Succeeded)
                    return false;

                foreach (var role in createUserDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
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
                if (user == null) return false;

                _userMapper.UpdateUserFromDto(updateUserDto, user);
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return false;

                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesToRemove = currentRoles.Except(updateUserDto.Roles).ToList();
                var rolesToAdd = updateUserDto.Roles.Except(currentRoles).ToList();

                if (rolesToRemove.Any())
                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                if (rolesToAdd.Any())
                    await _userManager.AddToRolesAsync(user, rolesToAdd);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user: {userId}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user: {userId}");
                return false;
            }
        }
    }
}
