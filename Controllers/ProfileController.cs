using System.Security.Claims;
using LoginEvaluation.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginEvaluation.Controllers;

[Authorize]
public class ProfileController : Controller
{
    public IActionResult Index()
    {
        var vm = new ProfileViewModel
        {
            Name = User.FindFirstValue(ClaimTypes.GivenName) ?? "Luis Eduardo",
            Username = User.FindFirstValue(ClaimTypes.Email) ?? "luis.eduardo.200325@gmail.com",
            DocumentType = User.FindFirst("document_type")?.Value ?? "DNI",
            Dni = User.FindFirst("dni")?.Value ?? "87654321",
            IsActive = (User.FindFirst("user_status")?.Value ?? string.Empty) == "Active"
        };

        return View(vm);
    }
}
