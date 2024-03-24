using System;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Suppliers;

namespace Nop.Web.Areas.Admin.Controllers;
public class SupplierHomeController : BaseSupplierController
{
    private readonly IOrderService _orderService;
    private readonly IReturnRequestService _returnRequestService;
    private readonly IProductService _productService;
    private readonly IWorkContext _workContext;

    public SupplierHomeController(IOrderService orderService,
        IReturnRequestService returnRequestService,
        IProductService productService,
        IWorkContext workContext)
    {
        _orderService = orderService;
        _returnRequestService = returnRequestService;
        _productService = productService;
        _workContext = workContext;
    }
    public async Task<IActionResult> Index()
    {
        var vendor = await _workContext.GetCurrentVendorAsync();
        var now = DateTime.UtcNow;
        var currentMonthStartDate = new DateTime(now.Year, now.Month, 1);
        var currentMonthEndDate = currentMonthStartDate.AddMonths(1).AddDays(-1);
        var lastMonthStartDate = currentMonthStartDate.AddMonths(-1);
        var lastMonthEndDate = currentMonthStartDate.AddDays(-1);

        var currentMonthOrders = (await _orderService.SearchOrdersAsync(pageIndex: 0, pageSize: 1, getOnlyTotalCount: true,
                createdFromUtc: currentMonthStartDate,
                createdToUtc: currentMonthEndDate,
                vendorId: vendor.Id))?.TotalCount??0;

        var lastMonthOrders = (await _orderService.SearchOrdersAsync(pageIndex: 0, pageSize: 1, getOnlyTotalCount: true,
            createdFromUtc: lastMonthStartDate,
            createdToUtc: lastMonthEndDate,
            vendorId: vendor.Id))?.TotalCount??0;

        var numberOfOrdersPercentage = lastMonthOrders == 0 ? 0 : ((currentMonthOrders - lastMonthOrders) / lastMonthOrders) * 100;

        var currentMonthIncome = await _orderService.GetOrdersIncomeAsync(createdFromUtc: currentMonthStartDate,
            createdToUtc: currentMonthEndDate,
            vendorId: vendor.Id);

        var lastMonthIncome = await _orderService.GetOrdersIncomeAsync(createdFromUtc: lastMonthStartDate,
            createdToUtc: lastMonthEndDate,
            vendorId: vendor.Id);

        var incomePercenctage = lastMonthIncome == 0 ? 0 : ((currentMonthIncome - lastMonthIncome) / lastMonthIncome) * 100;

        var returnRequestStatus = ReturnRequestStatus.Pending;
        var currentMonthReturns = (await _returnRequestService.SearchReturnRequestsAsync(rs: returnRequestStatus,
            pageIndex: 0, pageSize: 1, getOnlyTotalCount: true,
            createdFromUtc: currentMonthStartDate,
            createdToUtc: currentMonthEndDate)).TotalCount;

        var lastMonthReturns = (await _returnRequestService.SearchReturnRequestsAsync(rs: returnRequestStatus,
            pageIndex: 0, pageSize: 1, getOnlyTotalCount: true,
            createdFromUtc: lastMonthStartDate,
            createdToUtc: lastMonthEndDate)).TotalCount;

        var returnsPercentage = lastMonthReturns == 0 ? 0 : ((currentMonthReturns - lastMonthReturns) / lastMonthReturns) * 100;

        return View(new SupplierStatisticsModel
        {
            NumberOfOrders = currentMonthOrders,
            NumberOfOrdersPercentage = numberOfOrdersPercentage,
            TotalIncome = currentMonthIncome,
            TotalIncomePercentage = incomePercenctage,
            NumberOfSalesReturn = currentMonthReturns,
            NumberOfSalesReturnPercentage = returnsPercentage,
            NumberOfInvoices = currentMonthOrders,
            NumberOfInvoicesPercentge = numberOfOrdersPercentage
        });
    }

    public async Task<IActionResult> GetProductsAboutToExpire()
    {
        var vendor = await _workContext.GetCurrentVendorAsync();

        var result = await _productService.GetTop3AboutToExpireProducts(vendor.Id);

        return Json(result);
    }

    public async Task<IActionResult> GetMostActiveOrdersInCities(string city)
    {
        var vendor = await _workContext.GetCurrentVendorAsync();
        var result = await _orderService.SearchOrdersAsync(vendorId: vendor.Id, shippingCity: city, getOnlyTotalCount: true);
        return Json(result.TotalCount);
    }
}
