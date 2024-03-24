using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Core.Domain.Security;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.ScheduleTasks;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

namespace Sup.Plugin.Merchant
{
    public class Sup_plugin_Marchant : BasePlugin, IAdminMenuPlugin
    {

        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;



      

        public Sup_plugin_Marchant(IPermissionService permissionService,
            IWebHelper webHelper, ILocalizationService localizationService)
        {
            _permissionService = permissionService;
            _webHelper = webHelper;
            _localizationService = localizationService;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Contract/List";
        }

        public virtual async Task InstallAsync()
        {
            #region Add Locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["admin.onlineorder.exportexcel"] = "Export to excel",
                ["Confirmation.Detailes.Orders"] = "Detailes Orders",
                ["admin.onlineorder.exportexcel"] = "Export to excel",
                ["admin.onlineorder.exportexcel"] = "Export to excel",
                ["Confirmation.List.Of.Products"] = "List Of Products",
                ["Confirmation.sales.transaction.Delivery.Date"] = "Delivery Date",
                ["confirmation.sales.transaction.delivery.time"] = "Delivery Time",
                ["Confirmation.Total.Before.Tax"] = "Total Before Tax",
                ["Confirmation.Total.Discount"] = "Total Discount",
                ["Confirmation.Vat.Tax.Amount"] = "Vat Tax Amount",
                ["CreateOnlineOrder.Page.Site"] = "Create Online Order",
                ["Enums.Nop.Core.Domain.OnlineOrders.OnlineOrderStatus.Confirmed"] = "Confirmed",
                ["Enums.Nop.Core.Domain.OnlineOrders.OnlineOrderStatus.Received"] = "Received",
                ["Enums.Nop.Core.Domain.OnlineOrders.OnlineOrderStatus.Saved"] = "Saved",
                ["HomePage.OnlineOrdering.Description"] = "Create new sales order and view the status of the previously requested orders",
                ["HomePage.OnlineOrdering.Title"] = "Online Ordering",
                ["Is.Available"] = "Is Available",
                ["Last.3.Months"] = "Last 3 Month",
                ["List.Of.Order"] = "List Of Order",
                ["Net.Total"] = "Net Total",
                ["New.Order"] = "New Order",
                ["No online orders matches the search criteria"] = "No online orders matches search criteria",
                ["OnlineOrder.ActivityLog.Created"] = "Online Order Created",
                ["OnlineOrder.ActivityLog.Updated"] = "Online Order Updated",
                ["OnlineOrder.BackToList"] = "Back to list",
                ["OnlineOrder.BackToList.Admin"] = "Back to list",
                ["OnlineOrder.Delete.Confirm"] = "Are you sure youwant to delete this order?",
                ["onlineorder.deleted.success"] = "Online order deleted successfully.",
                ["OnlineOrder.Error.CustomerStatusNotActive"] = "We apologize that your request has not been fulfilled. Please contact 012 ",
                ["OnlineOrder.Error.NetTotalLessThan200"] = "Net Total amount must be more than or equal 200 SAR.",
                ["OnlineOrder.Error.ZeroQuantity"] = "Quantity need to be more than zero",
                ["OnlineOrder.Field.Branchcode.Admin"] = "Branch code",
                ["OnlineOrder.Field.ConfirmedById.Admin"] = "Confirmed By",
                ["onlineorder.field.confirmeditems.site"] = "Confirmed Items",
                ["OnlineOrder.Field.ConfirmedOnUtc.Admin"] = "Confirmed Date",
                ["onlineorder.field.createdby.site"] = "Created By",
                ["OnlineOrder.Field.CreatedById.Admin"] = "Created By",
                ["OnlineOrder.Field.CreatedOnTime.Site"] = "Time",
                ["OnlineOrder.Field.CreatedOnUtc.Admin"] = "Created Date",
                ["OnlineOrder.Field.CreatedOnUtc.site"] = "Created Date",
                ["OnlineOrder.Field.CreatedOnUtcFrom.Admin"] = "From Date",
                ["OnlineOrder.Field.CreatedOnUtcTo.Admin"] = "To Date",
                ["OnlineOrder.Field.CustomerCode.Admin"] = "Customer Code",
                ["OnlineOrder.Field.CustomerSegment.Admin"] = "Customer Segment",
                ["OnlineOrder.Field.Delete.Site"] = "Delete",
                ["onlineorder.field.details.site"] = "Details",
                ["OnlineOrder.Field.ItemsCount.Admin"] = "Items Count",
                ["OnlineOrder.Field.ItemsCount.Site"] = "Items Count",
                ["OnlineOrder.Field.OnlineOrderItems.Required"] = "Order items are required",
                ["OnlineOrder.Field.OrderNumber.Admin"] = "Serial Number",
                ["OnlineOrder.Field.OrderNumber.Site"] = "Order Number",
                ["OnlineOrder.Field.PharmacyName.Admin"] = "Pharmacy Name",
                ["onlineorder.field.pharmacyprice.admin"] = "Pharmacy Price",
                ["OnlineOrder.Field.PharmacyPrice.Site"] = "Pharmacy Price",
                ["OnlineOrder.Field.Phone.Admin"] = "Primary Mobile",
                ["OnlineOrder.Field.Price.Site"] = "Price",
                ["onlineorder.field.productcode.admin"] = "Item Code",
                ["onlineorder.field.productname.admin"] = "Item Name",
                ["OnlineOrder.Field.ProductName.Site"] = "Product",
                ["onlineorder.field.quantity.admin"] = "Price",
                ["OnlineOrder.Field.Quantity.Site"] = "Quantity",
                ["onlineorder.field.ref.number.admin"] = "Order Number",
                ["OnlineOrder.Field.RefNumber.Site"] = "Ref Number",
                ["OnlineOrder.Field.ReviewedById.Admin"] = "Reviewed By",
                ["OnlineOrder.Field.ReviewedOnUtc.Admin"] = "Reviewd Date",
                ["OnlineOrder.Field.SalesOrderNumber.Admin"] = "Sales Order Number",
                ["OnlineOrder.Field.SalesOrderNumber.MustNull"] = "Can not add Sales order number with not confirmed status",
                ["OnlineOrder.Field.SalesOrderNumber.MustUnique"] = "Sales Order Number Must Unique",
                ["OnlineOrder.Field.SalesOrderNumber.Required"] = "Sales order number is required",
                ["OnlineOrder.Field.SalesOrderNumber.Site"] = "Sales Order Number",
                ["OnlineOrder.Field.SerialNumber.Site"] = "Serial Number",
                ["OnlineOrder.Field.StatusId.Admin"] = "Order Status",
                ["OnlineOrder.Field.StatusId.NotValid"] = "Orders status is not valid",
                ["OnlineOrder.Field.StatusId.Site"] = "Order Status",
                ["onlineorder.field.subtotal.admin"] = "Sub Total",
                ["OnlineOrder.Field.TotalAmount.Admin"] = "Total Amount",
                ["OnlineOrder.Field.TotalAmount.Site"] = "Total Amount",
                ["OnlineOrder.Field.UpdatedOnUtc.Admin"] = "Update on",
                ["OnlineOrder.ItemStatus.Available"] = "Available",
                ["OnlineOrder.ItemStatus.AvailableQuota"] = "Available Quota",
                ["OnlineOrder.ItemStatus.MaxQuantity"] = "Max Quantity",
                ["OnlineOrder.ItemStatus.Unavailable"] = "Unavailable",
                ["OnlineOrder.Page.Create.Site"] = "Create New Online Order",
                ["OnlineOrder.Page.Create.Site"] = "Create New Online Order",
                ["OnlineOrder.Page.Details.Save.Site"] = "Save",
                ["OnlineOrder.Page.Details.Site"] = "Online Order Details",
                ["OnlineOrder.Page.Details.Submit.Site]"] = "Submit",
                ["OnlineOrder.Page.Edit.Admin"] = "Edit Online Order",
                ["OnlineOrder.Page.Edit.Site"] = "Edit Online Order",
                ["OnlineOrder.Page.List.Admin"] = "Online Orders",
                ["OnlineOrder.Page.List.Site"] = "Online Orders",
                ["OnlineOrder.Page.SubmitConfirmation"] = "Order quantity may vary according to availability when submitted, Do you want to proceed saving ?",
                ["OnlineOrder.Saved.Success"] = "Online Order Saved Successfully",
                ["onlineorder.search.all"] = "All",
                ["OnlineOrder.SearchProduct"] = "Find Product",
                ["OnlineOrder.SearchType.Contain"] = "Contain",
                ["OnlineOrder.SearchType.StartWith"] = "Start With",
                ["OnlineOrder.Submit.Confirm"] = "Are you sure you want to submit?",
                ["onlineorder.submited.success"] = "online order submitted successfully",
                ["OnlineOrder.Submitted.Success"] = "Online Order Submitted Successfully",
                ["OnlineOrder.Updated.Success"] = "Online Order Updated Successfully",
                ["OnlineOrderList.Page.List.Site"] = "My Online Orders",
                ["onlineorders.backtolist"] = "Back To List",
                ["Order.Number"] = "Order Number",
                ["QTY.are.updated.based.on.availability"] = "QTY are updated based on availability",
                ["Received"] = "Received",
                ["Recieved"] = "Recieved",
                ["Requested.Products"] = "Requested Products",
                ["Saved"] = "Saved",
                ["Submitted"] = "Submitted",
                ["You don't have online orders yet"] = "You don't have online orders yet",
            });

            await base.InstallAsync();



            #endregion



            await base.InstallAsync();
        }

        public virtual Task UninstallAsync()
        {
            return Task.CompletedTask;
        }
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return;

            var config = rootNode.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Configuration"));
            if (config == null)
                return;

            var plugins = config.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Local plugins"));

            if (plugins == null)
                return;

            var index = config.ChildNodes.IndexOf(plugins);

            if (index < 0)
                return;

            config.ChildNodes.Insert(index, new SiteMapNode
            {
                SystemName = "nopCommerce Merchant plugin",
                Title = "Merchant",
                ControllerName = "Contract",
                ActionName = "List",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
            });
        }
    }
    
}
