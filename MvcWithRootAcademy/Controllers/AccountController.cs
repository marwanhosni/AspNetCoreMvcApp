using Microsoft.AspNetCore.Mvc;

namespace MvcWithRootAcademy.Controllers;


public class AccountController : Controller
{
    #region Signup
    public IActionResult Signup()
    {
        return View("Signup");
    }
    #endregion
}
