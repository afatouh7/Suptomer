using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers;

public class MerchantInvoicesController : BaseMerchantController
{
    public IActionResult Index()
    {
        return View();
    }
}
