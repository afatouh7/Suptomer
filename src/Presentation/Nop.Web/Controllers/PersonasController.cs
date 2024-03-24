using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers;

public class PersonasController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
