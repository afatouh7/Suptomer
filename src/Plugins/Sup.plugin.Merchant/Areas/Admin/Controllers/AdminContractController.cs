using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Sup.Plugin.Merchant.Areas.Admin.Models;

namespace Sup.Plugin.Merchant.Areas.Admin.Controllers
{
    public class AdminContractController : BaseAdminController
    {
        public AdminContractController()
        {

        }


        public virtual IActionResult Index()
        {
            return RedirectToAction("List", new { area = AreaNames.Admin });
        }
        public virtual IActionResult List()
        {
            var model = new ContractModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult List(ContractModel searchModel)
        {

            //prepare model
            var model = new ContractModel();

            return Json(model);
        }

        public virtual IActionResult Edit(int id)
        {
            var model = new ContractModel();

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ContractModel model, bool continueEditing)
        {

            //prepare model
            model = new ContractModel();

            //if we got this far, something failed, redisplay form
            return View(model);
        }


    }
}
