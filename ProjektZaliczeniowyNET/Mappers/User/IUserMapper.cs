using Microsoft.AspNetCore.Identity;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.User;

namespace ProjektZaliczeniowyNET.Mappers
{
    public interface IUserMapper
    {
        UserDto ToUserDto(User user, IList<string>? roles = null);
        UserListDto ToUserListDto(User user, IList<string>? roles = null);
        User ToUser(UserCreateDto dto);
        void UpdateUserFromDto(UserUpdateDto dto, User user);
        List<UserDto> ToUserDtoList(IEnumerable<User> users, Func<User, IList<string>>? getRoles = null);
        List<UserListDto> ToUserListDtoList(IEnumerable<User> users, Func<User, IList<string>>? getRoles = null);
        UserUpdateDto ToUserUpdateDto(UserDto userDto);
        User CreateUser(UserCreateDto dto, string? passwordHash = null);
    }
}