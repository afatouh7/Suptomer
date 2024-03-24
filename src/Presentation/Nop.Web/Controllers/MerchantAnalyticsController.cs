using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers;

public class MerchantAnalyticsController : BaseMerchantController
{
    public IActionResult Index()
    {
        return View();
    }
}
