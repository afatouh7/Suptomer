using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Customers;
using Nop.Web.Factories;
using Nop.Web.Models.Supplier;

namespace Nop.Web.Controllers;

public class MerchantPaymentsController : BaseMerchantController
{
    private readonly ICustomerService _customerService;
    private readonly IWorkContext _workContext;
    private readonly IOrderModelFactory _orderModelFactory;

    public MerchantPaymentsController(ICustomerService customerService,
        IWorkContext workContext,
        IOrderModelFactory orderModelFactory)
    {
        _customerService = customerService;
        _workContext = workContext;
        _orderModelFactory = orderModelFactory;
    }
    public async Task<IActionResult> Index()
    {
        if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var model = await _orderModelFactory.PreparePaymentSearchModelAsync(new PaymentSearchModel());
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllPayments(PaymentSearchModel searchModel)
    {
        var model = await _orderModelFactory.PreparePaymentListModelAsync(searchModel);
        return Json(model);
    }
}
