﻿using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        public virtual IActionResult Index()
        {
            return View();
        }
        
        public IActionResult MerchantPage()
        {
            return View();
        }

        public IActionResult SupplierPage()
        {
            return View();
        }
    }
}