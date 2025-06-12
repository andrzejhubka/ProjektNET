using ProjektZaliczeniowyNET.DTOs.User;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Mappers
{
    public class UserMapper
    {
        public UserDto ToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmployeeNumber = user.EmployeeNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Notes = user.Notes,
                Roles = new List<string>() // Role są dodawane później w serwisie
            };
        }

        public User ToApplicationUser(UserCreateDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                EmployeeNumber = dto.EmployeeNumber,
                Notes = dto.Notes
                // IsActive, CreatedAt są ustawiane w serwisie
            };
        }

        public void UpdateUserFromDto(UserUpdateDto dto, User user)
        {
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.EmployeeNumber = dto.EmployeeNumber;
            user.IsActive = dto.IsActive;
            user.Notes = dto.Notes;
        }
    }
}