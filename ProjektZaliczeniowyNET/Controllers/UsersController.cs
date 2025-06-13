using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.DTOs.User;

namespace ProjektZaliczeniowyNET.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserMapper _userMapper;

        public UsersController(IUserService userService, UserMapper userMapper)
        {
            _userService = userService;
            _userMapper = userMapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto createUserDto)  // <-- UserCreateDto, nie CreateUserDto
        {
            if (!ModelState.IsValid)
            {
                return View(createUserDto);
            }

            var success = await _userService.CreateUserAsync(createUserDto);

            if (success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Nie udało się utworzyć użytkownika.");
            return View(createUserDto);
        }
    }
}