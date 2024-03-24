using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Areas.Admin.Controllers;
public class SupplierInventoryController : BaseSupplierController
{
    public IActionResult Index()
    {
        return View();
    }
}
