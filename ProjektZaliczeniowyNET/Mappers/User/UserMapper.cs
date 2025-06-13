using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.DTOs.User;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        // Mapowanie User -> UserDto (z rolami jako dodatkowy argument)
        public partial UserDto ToUserDto(User user, IList<string> roles);

        // Mapowanie User -> UserListDto (z rolami jako dodatkowy argument)
        public partial UserListDto ToUserListDto(User user, IList<string> roles);

        // Mapowanie UserCreateDto -> User (utworzenie encji z DTO)
        public partial User ToUser(UserCreateDto createDto);

        // Aktualizacja encji User na podstawie UserUpdateDto
        public partial void UpdateUserFromDto(UserUpdateDto updateDto, User user);
    }
}