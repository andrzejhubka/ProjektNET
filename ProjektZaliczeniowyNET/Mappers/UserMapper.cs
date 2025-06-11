// Mappers/UserMapper.cs
using Riok.Mapperly.Abstractions;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs;

namespace ProjektZaliczeniowyNET.Mappers;

[Mapper]
public partial class UserMapper
{
    // Mapowanie z User do UserDto
    public partial UserDto ToUserDto(User user);
    
    // Mapowanie z CreateUserDto do ApplicationUser
    [MapProperty(nameof(CreateUserDto.Email), nameof(User.UserName))]
    public partial User ToApplicationUser(CreateUserDto createUserDto);
    // DODAJ TĘ METODĘ:
    public partial void UpdateUserFromDto(UpdateUserDto updateDto, User user);
}