using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Contract;
using Nop.Services.Catalog;
using Nop.Web.Models.Supplier;
using Sup.Plugin.Suppliert.Areas.Admin.Models;

namespace Nop.Web.Areas.Admin.Controllers;

public class SupplierAnalyticsController : BaseSupplierController
{
    private readonly IProductService _productService;
    public SupplierAnalyticsController(IProductService productService)
    {
        _productService = productService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ContractDetails()
        {
            return View();
        }

    public IActionResult OfferInformation()
        {
            return View();
        }

    public async Task<IActionResult> SupplierContractsList()
    {
        var model = new ContractModel();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> SupplierContractsList(SupplierSearchModel searchModel)
    {
        var model = new ContractModel();
        return Json(model);
    }
    //public async Task<IActionResult> AltenativeProducts(int productId)
    //{
    //    var producatCategory = await _productService.GetProductCategoryByProductIdAsync(productId);
    //    if (producatCategory == null)
    //        return NotFound();

    //    var model = await _productService.GetCategoryFeaturedProductsAsync(producatCategory.CategoryId, 0);

    //    return View(model);
    //}
    //[HttpPost]
    //public async Task<IActionResult> AddedSuplier(SupplierContract supplierContract)
    //{
    //    var model = _productService.AddSuplierContractAsync(supplierContract);
    //    return Json(model);
    //}
}
