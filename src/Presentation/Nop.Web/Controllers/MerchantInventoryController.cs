using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers;

public class MerchantInventoryController : BaseMerchantController
{
    public IActionResult Index()
    {
        return View();
    }
}
