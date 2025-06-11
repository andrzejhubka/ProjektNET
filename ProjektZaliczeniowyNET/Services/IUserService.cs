using ProjektZaliczeniowyNET.DTOs;

namespace ProjektZaliczeniowyNET.Services;

public interface IUserService
{
    // Operacje crud
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<bool> CreateUserAsync(CreateUserDto createUserDto);
    Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(string userId);
}