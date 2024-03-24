using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Vendors;
using Nop.Services.Orders;
using Nop.Services.Shipping;

namespace Nop.Web.Controllers;
public class MerchantHomeController : BaseMerchantController
{
    private readonly IWorkContext _workContext;
    private readonly IStoreContext _storeContext;
    private readonly IOrderService _orderService;
    private readonly IShipmentService _shipmentService;

    public MerchantHomeController(IWorkContext workContext,
        IStoreContext storeContext,
        IOrderService orderService,
        IShipmentService shipmentService)
    {
        _workContext = workContext;
        _storeContext = storeContext;
        _orderService = orderService;
        _shipmentService = shipmentService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetTopCardsDetais()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var orders = await _orderService.SearchOrdersAsync(storeId: store.Id,
            customerId: customer.Id, getOnlyTotalCount: true);

        var transitShipments = await _shipmentService.GetAllShipmentsAsync(
            customerId: customer.Id,
            loadNotShipped: true,
            loadNotDelivered: true,
            pageIndex: 0,
            pageSize: 1,
            getOnlyTotalCount: true);

        var deliveredShipments = await _shipmentService.GetAllShipmentsAsync(
            customerId: customer.Id,
            loadIsDelivered: true,
            pageIndex: 0,
            pageSize: 1,
            getOnlyTotalCount: true);

        var result = new MerchantDashbardTopCard
        {
            TotalOrders = orders.TotalCount,
            TotalTransit = transitShipments.TotalCount,
            TotalDelivered = deliveredShipments.TotalCount,
        };

        return Json(result);
    }
}

public class MerchantDashbardTopCard
{
    public int TotalOrders { get; set; }
    public int TotalTransit { get; set; }
    public int TotalDelivered { get; set; }
}
