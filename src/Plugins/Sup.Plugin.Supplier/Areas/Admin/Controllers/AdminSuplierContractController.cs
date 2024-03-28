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
using Nop.Web.Models.Supplier;
using Sup.Plugin.Supplier.Models;

namespace Sup.Plugin.Supplier.Areas.Admin.Controllers
{
    public class AdminSuplierContractController : BaseSupplierController
    {


        public AdminSuplierContractController()
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

        public async Task<IActionResult> SupplierContractsList()
        {
            var model = new ContractModel();
            return View("~/Plugins/Sup.Plugin.Supplier/Views/SupplierContractsList.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> SupplierContractsList(SupplierSearchModel searchModel)
        {
            var model = new ContractModel();
            return Json(model);
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
