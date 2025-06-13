using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.DTOs.User;
using ProjektZaliczeniowyNET.Mappers;
using ProjektZaliczeniowyNET.Services;

namespace ProjektZaliczeniowyNET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserMapper _userMapper;

        public UsersController(IUserService userService, UserMapper userMapper)
        {
            _userService = userService;
            _userMapper = userMapper;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetUsersForListAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _userService.CreateUserAsync(createUserDto);
            if (!success)
                return BadRequest("Nie udało się utworzyć użytkownika.");

            return CreatedAtAction(nameof(GetById), new { id = createUserDto.Email }, createUserDto);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _userService.UpdateUserAsync(id, updateUserDto);
            if (!success)
                return NotFound("Nie znaleziono użytkownika lub nie udało się zaktualizować.");

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound("Nie znaleziono użytkownika lub nie udało się usunąć.");

            return NoContent();
        }

        // POST: api/users/{id}/toggle-status
        [HttpPost("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var success = await _userService.ToggleUserStatusAsync(id);
            if (!success)
                return NotFound("Nie znaleziono użytkownika lub nie udało się zmienić statusu.");

            return NoContent();
        }
    }
}
