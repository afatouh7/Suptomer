using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using Nop.Web.Factories;
using Nop.Web.Models.Supplier;
using Sup.Plugin.Suppliert.Areas.Admin.Models;

namespace Nop.Web.Controllers;
public class MerchantSuppliersController : BaseMerchantController
{
    private readonly ICustomerService _customerService;
    private readonly ICustomerModelFactory _customerModelFactory;

    public MerchantSuppliersController(ICustomerService customerService,
        ICustomerModelFactory customerModelFactory)
    {
        _customerService = customerService;
        _customerModelFactory = customerModelFactory;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _customerModelFactory.PrepareSupplierSearchModelAsync(new SupplierSearchModel());
        return View(model);
    }

    //public async Task<IActionResult> GetAllSuppliersForMerchants()
    //{
    //    var model = await _customerModelFactory.PrepareSupplierSearchModelAsync(new SupplierSearchModel());
    //    return View(nameof(Index), model);
    //}

    [HttpPost]
    public async Task<IActionResult> GetAllSuppliersForMerchants(SupplierSearchModel searchModel)
    {
        var model = await _customerModelFactory.PrepareSupplierListModelAsync(searchModel);
        return Json(model);
    }

  
}
