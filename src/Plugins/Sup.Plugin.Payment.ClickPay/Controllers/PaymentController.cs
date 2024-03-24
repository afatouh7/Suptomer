using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Sup.Plugin.Payment.ClickPay.Models;
using Sup.Plugin.Payment.ClickPay.Helpers;
using Nop.Services.Security;
using Nop.Core;
using Nop.Web.Framework.Controllers;
using Nop.Services.Configuration;
using Nop.Plugin.Payments.Manual;
using Nop.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Core.Domain.Orders;
using Nop.Services.Orders;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Sup.Plugin.Payment.ClickPay.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class PaymentController: BasePaymentController
    {
       // private readonly ClickPayApiClient _clickPayApiClient;
        private readonly IPermissionService _permissionService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;
        public PaymentController(ILocalizationService localizationService,
            ILanguageService languageService,IPermissionService permissionService, IStoreContext storeContext,
            ISettingService settingService, INotificationService notificationService, IOrderService orderService,
            ShoppingCartSettings shoppingCartSettings,
            IWorkContext workContext)
        {
           // _clickPayApiClient = clickPayApiClient ?? throw new ArgumentNullException(nameof(clickPayApiClient));
            _permissionService = permissionService;
            _storeContext = storeContext;
            _settingService = settingService;
            _localizationService = localizationService;
            _languageService = languageService;
            _notificationService = notificationService;
            _shoppingCartSettings = shoppingCartSettings;
            _orderService = orderService;
            _workContext = workContext;
        }






        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var clickPaySettings = await _settingService.LoadSettingAsync<ClickPayPaymentSettings>(storeScope);

            var model = new ClickPayConfiguration
            {
                
                ProfileId = clickPaySettings.ProfileId,
                ReturnUrl = clickPaySettings.ReturnUrl,
                ProfileServerKey = clickPaySettings.ProfileServerKey,
                CallbackUrl = clickPaySettings.CallbackUrl,
                ActiveStoreScopeConfiguration = storeScope

            };
        



            if (storeScope > 0)
            {
                model.ProfileId_OverrideForStore = await _settingService.SettingExistsAsync(clickPaySettings, x => x.ProfileId, storeScope);
                model.CallbackUrl_OverrideForStore = await _settingService.SettingExistsAsync(clickPaySettings, x => x.CallbackUrl, storeScope);
                model.ReturnUrl_OverrideForStore = await _settingService.SettingExistsAsync(clickPaySettings, x => x.ReturnUrl, storeScope);
                model.ProfileServerKey_OverrideForStore = await _settingService.SettingExistsAsync(clickPaySettings, x => x.ProfileServerKey, storeScope);
                
            }

            return View("~/Plugins/Sup.Plugin.Payment.ClickPay/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ClickPayConfiguration model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var clickPaySettings = await _settingService.LoadSettingAsync<ClickPayPaymentSettings>(storeScope);

            clickPaySettings.ProfileId = model.ProfileId;
            clickPaySettings.CallbackUrl = model.CallbackUrl;
            clickPaySettings.ReturnUrl = model.ReturnUrl;
            clickPaySettings.ProfileServerKey = model.ProfileServerKey;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */

            await _settingService.SaveSettingOverridablePerStoreAsync(clickPaySettings, x => x.ProfileId, model.ProfileId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(clickPaySettings, x => x.CallbackUrl, model.CallbackUrl_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(clickPaySettings, x => x.ReturnUrl, model.ReturnUrl_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(clickPaySettings, x => x.ProfileServerKey, model.ProfileServerKey_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }




        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> RoundingWarning(bool passProductNamesAndTotals)
        {
            if (! await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //prices and total aren't rounded, so display warning
            if (passProductNamesAndTotals && !_shoppingCartSettings.RoundPricesDuringCalculation)
                return Json(new { Result = await _localizationService.GetResourceAsync("Sup.Plugin.Payment.ClickPay.RoundingWarning") });

            return Json(new { Result = string.Empty });
        }

        public async Task<IActionResult> CancelOrder()
        {
            var order = await _orderService.SearchOrdersAsync(storeId: _storeContext.GetCurrentStore().Id,
                customerId: _workContext.GetCurrentCustomerAsync().Id, pageSize: 1);
            if (order != null)
                return RedirectToRoute("OrderDetails", new { orderId = order.FirstOrDefault().Id });

            return RedirectToRoute("HomePage");
        }
        //public async Task<IActionResult> Index()
        //{
        //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
        //        return AccessDeniedView();

        //    return View("~/Plugins/Payment.ClickPay/Views/Index.cshtml");
        //}

        //[HttpPost]
        //public async Task<IActionResult> ProcessPayment(ClickPayRequestModel requestModel)
        //{
            
           
        //    var serverKey = "S6JNLT9RJG-JHM9MKJB6J-B9LBLRZJBW";

        //    // Send payment request
        //   // var response = await _clickPayApiClient.SendPaymentRequestAsync(serverKey, requestModel);

        //    // Process response as needed
        //    return View("PaymentResponse", response);
        //}
    }
}

