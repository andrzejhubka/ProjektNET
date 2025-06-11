using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.DTOs;
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
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.Roles = roles;
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
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles;

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {UserId}", userId);
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
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles;

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var user = _userMapper.ToApplicationUser(createUserDto);
                user.CreatedAt = DateTime.UtcNow;
                user.IsActive = true;

                var result = await _userManager.CreateAsync(user, createUserDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, createUserDto.Role);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                _userMapper.UpdateUserFromDto(updateUserDto, user);

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
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
                if (user == null) return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                return false;
            }
        }
    }
}
