using ProjektZaliczeniowyNET.DTOs.User;

namespace ProjektZaliczeniowyNET.Services;

public interface IUserService
{
    // Operacje crud
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<bool> CreateUserAsync(UserCreateDto createUserDto);
    Task<bool> UpdateUserAsync(string userId, UserUpdateDto updateUserDto);
    Task<bool> DeleteUserAsync(string userId);
}