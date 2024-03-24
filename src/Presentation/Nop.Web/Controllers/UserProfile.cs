using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers
{
    public class UserProfile : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SupplierProfile()
        {
            return View();
        }
        public IActionResult MerchantProfile()
        {
            return View();
        }
    }
}
