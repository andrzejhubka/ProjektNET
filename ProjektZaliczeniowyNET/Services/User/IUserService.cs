using ProjektZaliczeniowyNET.DTOs.User;

namespace ProjektZaliczeniowyNET.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserListDto>> GetUsersForListAsync();
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<bool> CreateUserAsync(UserCreateDto createUserDto);
        Task<bool> UpdateUserAsync(string userId, UserUpdateDto updateUserDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> ToggleUserStatusAsync(string userId);
    }
}
