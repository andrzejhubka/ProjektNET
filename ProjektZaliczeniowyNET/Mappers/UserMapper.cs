using Microsoft.AspNetCore.Identity;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.DTOs.User;

namespace ProjektZaliczeniowyNET.Mappers
{
    public static class UserMapper
    {
        // Mapowanie z User do UserDto
        public static UserDto ToUserDto(User user, IList<string>? roles = null)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                EmployeeNumber = user.EmployeeNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Notes = user.Notes,
                Roles = roles ?? new List<string>()
            };
        }

        // Mapowanie z User do UserListDto
        public static UserListDto ToUserListDto(User user, IList<string>? roles = null)
        {
            return new UserListDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                EmployeeNumber = user.EmployeeNumber,
                IsActive = user.IsActive,
                Roles = roles ?? new List<string>()
            };
        }

        // Mapowanie z UserCreateDto do User
        public static User ToUser(UserCreateDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email, // W ASP.NET Identity UserName często jest tym samym co Email
                PhoneNumber = dto.PhoneNumber,
                EmployeeNumber = dto.EmployeeNumber,
                Notes = dto.Notes,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Aktualizacja User z UserUpdateDto
        public static void UpdateUserFromDto(UserUpdateDto dto, User user)
        {
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.UserName = dto.Email; // Synchronizacja UserName z Email
            user.PhoneNumber = dto.PhoneNumber;
            user.EmployeeNumber = dto.EmployeeNumber;
            user.IsActive = dto.IsActive;
            user.Notes = dto.Notes;
        }

        // Mapowanie listy User do listy UserDto
        public static List<UserDto> ToUserDtoList(IEnumerable<User> users, Func<User, IList<string>>? getRoles = null)
        {
            return users.Select(user => ToUserDto(user, getRoles?.Invoke(user))).ToList();
        }

        // Mapowanie listy User do listy UserListDto
        public static List<UserListDto> ToUserListDtoList(IEnumerable<User> users, Func<User, IList<string>>? getRoles = null)
        {
            return users.Select(user => ToUserListDto(user, getRoles?.Invoke(user))).ToList();
        }

        // Mapowanie z UserDto do UserUpdateDto (dla formularzy edycji)
        public static UserUpdateDto ToUserUpdateDto(UserDto userDto)
        {
            return new UserUpdateDto
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                EmployeeNumber = userDto.EmployeeNumber,
                IsActive = userDto.IsActive,
                Notes = userDto.Notes,
                Roles = userDto.Roles.ToList()
            };
        }

        // Pomocnicza metoda do tworzenia User z pełną konfiguracją
        public static User CreateUser(UserCreateDto dto, string? passwordHash = null)
        {
            var user = ToUser(dto);
            if (!string.IsNullOrEmpty(passwordHash))
            {
                user.PasswordHash = passwordHash;
            }
            return user;
        }
    }
}