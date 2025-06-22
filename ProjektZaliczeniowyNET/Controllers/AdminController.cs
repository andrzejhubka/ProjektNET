using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.ViewModels;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: Panel admina - lista użytkowników i ról
    public async Task<IActionResult> UsersRoles()
    {
        var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        var users = _userManager.Users.ToList();

        var model = new List<UserWithRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserWithRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                AllRoles = allRoles,
                SelectedRole = roles.FirstOrDefault() ?? ""
            });
        }

        return View(model);
    }

    // POST: Zmiana roli użytkownika
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUserRole(string userId, string selectedRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var currentRoles = await _userManager.GetRolesAsync(user);

        // Usuń wszystkie obecne role
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // Dodaj nową rolę, jeśli wybrano
        if (!string.IsNullOrEmpty(selectedRole))
            await _userManager.AddToRoleAsync(user, selectedRole);

        return RedirectToAction("UsersRoles");
    }
}