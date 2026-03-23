using System.Security.Claims;
using System.Threading.Tasks;
using LoginEvaluation.Application.Auth;
using LoginEvaluation.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LoginEvaluation.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        var model = new LoginViewModel();
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.AuthenticateAsync(model.Dni, model.Password, HttpContext.RequestAborted);
        if (result.Locked)
        {
            var lockVm = new LockoutViewModel
            {
                Email = model.Dni,
                LockoutEndUtc = result.LockoutEndUtc
            };
            return View("Locked", lockVm);
        }

        if (result.Inactive)
        {
            ModelState.AddModelError(string.Empty, "Acceso denegado: usuario inactivo.");
            return View(model);
        }

        if (!result.Success || result.User is null)
        {
            ModelState.AddModelError(string.Empty, "DNI o contraseña incorrectos.");
            return View(model);
        }

        var displayName = "Luis Eduardo";
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString()),
            new Claim("dni", result.User.Dni),
            new Claim(ClaimTypes.Email, result.User.Email),
            new Claim(ClaimTypes.Name, displayName),
            new Claim(ClaimTypes.GivenName, displayName),
            new Claim("document_type", model.DocumentType),
            new Claim("user_status", result.User.IsActive ? "Active" : "Inactive")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Profile");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> KeepAlive()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return Unauthorized();
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(User));
        return Ok();
    }

    [HttpPost]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> LogoutIdle()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}
