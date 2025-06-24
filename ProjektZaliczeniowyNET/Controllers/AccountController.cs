using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using ProjektZaliczeniowyNET.Constants;
using ProjektZaliczeniowyNET.ViewModels;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.Services;

namespace ProjektZaliczeniowyNET.Controllers
{
    public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public AccountController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = CreateApplicationUser(model);
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
            return await HandleSuccessfulRegistration(user, model.Email);

        AddErrorsToModelState(result.Errors);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (!await IsUserEmailConfirmed(user))
        {
            ModelState.AddModelError(string.Empty, ErrorMessages.EmailNotConfirmed);
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
            return RedirectToLocal(returnUrl);

        ModelState.AddModelError(string.Empty, ErrorMessages.InvalidLogin);
        return View(model);
    }

    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            return View(ViewNames.Error);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return View(ViewNames.Error);

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return View(result.Succeeded ? ViewNames.EmailConfirmed : ViewNames.Error);
    }

    // Private helper methods
    private ApplicationUser CreateApplicationUser(RegisterViewModel model)
    {
        return new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };
    }

    private async Task<IActionResult> HandleSuccessfulRegistration(ApplicationUser user, string email)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action("ConfirmEmail", "Account",
            new { userId = user.Id, token }, Request.Scheme);

        await _emailSender.SendEmailAsync(email, "Potwierdź swój email",
            $"Kliknij <a href='{confirmationLink}'>tutaj</a> aby potwierdzić swój email.");

        return View(ViewNames.EmailConfirmationSent);
    }

    private async Task<bool> IsUserEmailConfirmed(ApplicationUser user)
    {
        return user != null && await _userManager.IsEmailConfirmedAsync(user);
    }

    private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
            ModelState.AddModelError(string.Empty, error.Description);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        return Url.IsLocalUrl(returnUrl) 
            ? Redirect(returnUrl) 
            : RedirectToAction("Index", "Home");
    }
}

}