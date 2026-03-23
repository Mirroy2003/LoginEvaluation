using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginEvaluation.Controllers;

[AllowAnonymous]
public class WelcomeController : Controller
{
    [HttpGet]
    public IActionResult Activated()
    {
        return View();
    }
}

