using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.Manual;
using Nop.Plugin.Payments.Manual.Components;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Sup.Plugin.Payment.ClickPay.Helpers;
using Sup.Plugin.Payment.ClickPay.Models;
using Sup.Plugin.Payment.ClickPay.Validators;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace Sup.Plugin.Payment.ClickPay
{
    /// <summary>
    /// Manual payment processor
    /// </summary>
    public class PaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
       
        private readonly HttpClient _httpClient;
        private readonly ClickPayPaymentService _clickPayPaymentService;
        private readonly ClickPayPaymentSettings _clickPayPaymentSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Ctor

        public PaymentProcessor(ILocalizationService localizationService,
            IOrderTotalCalculationService orderTotalCalculationService,
            ISettingService settingService,
            IWebHelper webHelper,
            IHttpClientFactory httpClientFactory,
            ClickPaySettings manualPaymentSettings,
            HttpClient httpClient,
            ClickPayPaymentService clickPayPaymentService,
            ClickPayPaymentSettings clickPayPaymentSettings
            )
        {
            _localizationService = localizationService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _settingService = settingService;
            _webHelper = webHelper;
          
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://secure.clickpay.com.sa/");
            _clickPayPaymentService = clickPayPaymentService;
            _clickPayPaymentSettings = clickPayPaymentSettings;
            _httpClientFactory = httpClientFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the process payment result
        /// </returns>
        public async  Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest paymentRequest)
        {

            var httpClient = _httpClientFactory.CreateClient();
            var requestUrl = "https://secure.clickpay.com.sa/payment/request";
            var serverKey = _clickPayPaymentSettings.ProfileServerKey;
            var requestData = new
            {
                profile_id = _clickPayPaymentSettings.ProfileId,
                tran_type = "sale",
                tran_class = "ecom",
                cart_id = paymentRequest.CreditCardNumber,
                cart_description = paymentRequest.CreditCardName,
                cart_currency = "SAR",
                cart_amount = paymentRequest.OrderTotal,
                callback = _clickPayPaymentSettings.CallbackUrl,
                returnUrl= _clickPayPaymentSettings.ReturnUrl,
                 customer_details = paymentRequest.CustomValues,
             

            };

        var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", serverKey);


            var response = await httpClient.PostAsync(requestUrl, requestContent);

    if (response.IsSuccessStatusCode)
          {
        var responseContent = await response.Content.ReadAsStringAsync();
        var paymentResult = JsonConvert.DeserializeObject<ProcessPaymentResult>(responseContent);
        return paymentResult;
          }
    else
         {
        // Handle unsuccessful API call
        throw new Exception($"Failed to process payment. Status code: {response.StatusCode}");
        }
    }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            //nothing
            var httpClient = _httpClientFactory.CreateClient();
            var requestUrl = "https://secure.clickpay.com.sa/payment/request";
            var serverKey = _clickPayPaymentSettings.ProfileServerKey;
            var requestData = new
            {
                profile_id = _clickPayPaymentSettings.ProfileId,
                tran_type = "sale",
                tran_class = "ecom",
                cart_id = postProcessPaymentRequest.Order.CardNumber,
                cart_description = postProcessPaymentRequest.Order.CardName,
                cart_currency = "SAR",
                cart_amount = postProcessPaymentRequest.Order.OrderTotal,
                callback = _clickPayPaymentSettings.CallbackUrl,
                returnUrl = _clickPayPaymentSettings.ReturnUrl,
                customer_details = postProcessPaymentRequest.Order.CustomerId,


            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", serverKey);


            var response = await httpClient.PostAsync(requestUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var paymentResult = JsonConvert.DeserializeObject<ProcessPaymentResult>(responseContent);
               
            }
            else
            {
                // Handle unsuccessful API call
                throw new Exception($"Failed to process payment. Status code: {response.StatusCode}");
            }


        }
       
        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - hide; false - display.
        /// </returns>
        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return Task.FromResult(false);
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the additional handling fee
        /// </returns>
        public  Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(decimal.Zero);
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the capture payment result
        /// </returns>
        public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
        {
            return Task.FromResult(new CapturePaymentResult { Errors = new[] { "Capture method not supported" } });
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var requestUrl = "https://secure.clickpay.com.sa/payment/refund";

            var requestData = new
            {
                profile_id = _clickPayPaymentSettings.ProfileId,
                order_id = refundPaymentRequest.Order.Id,
                refund_amount = refundPaymentRequest.AmountToRefund
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(requestUrl, requestContent).Result; // Using .Result to synchronously wait for the response

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result; // Using .Result to synchronously read the response content
                var refundResult = JsonConvert.DeserializeObject<RefundResult>(responseContent);
                return Task.FromResult(new RefundPaymentResult
                {
                    NewPaymentStatus = PaymentStatus.Refunded
                });
            }
            else
            {
                // Handle unsuccessful API call
                var errorMessage = $"Failed to process refund. Status code: {response.StatusCode}";
                return Task.FromResult(new RefundPaymentResult
                {
                    Errors = new List<string> { errorMessage }
                });
            }
        }

            /// <summary>
            /// Voids a payment
            /// </summary>
            /// <param name="voidPaymentRequest">Request</param>
            /// <returns>
            /// A task that represents the asynchronous operation
            /// The task result contains the result
            /// </returns>
            public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            return Task.FromResult(new VoidPaymentResult { Errors = new[] { "Void method not supported" } });
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the process payment result
        /// </returns>
        public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } });
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            //always success
            return Task.FromResult(new CancelRecurringPaymentResult());
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public Task<bool> CanRePostProcessPaymentAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //it's not a redirection payment method. So we always return false
            return Task.FromResult(false);
        }

        /// <summary>
        /// Validate payment form
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of validating errors
        /// </returns>
        public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            var warnings = new List<string>();

            //validate
            var validator = new PaymentValidator(_localizationService);
            var model = new PaymentInfo
            {
                card_scheme = form["card_type"],
                card_type = form["CardNumber"],
                payment_description = form["payment_description"],
                expiryYear = form["expiryYear"],
                expiryMonth = form["expiryMonth"]
            };
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
                warnings.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

            return Task.FromResult<IList<string>>(warnings);
        }

        /// <summary>
        /// Get payment information
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the payment info holder
        /// </returns>
        public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            return Task.FromResult(new ProcessPaymentRequest
            {
                CreditCardType = form["CreditCardType"],
                CreditCardName = form["CardholderName"],
                CreditCardNumber = form["CardNumber"],
                CreditCardExpireMonth = int.Parse(form["ExpireMonth"]),
                CreditCardExpireYear = int.Parse(form["ExpireYear"]),
                CreditCardCvv2 = form["CardCode"]
            });
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/Payment/Configure";
        }

        /// <summary>
        /// Gets a type of a view component for displaying plugin in public store ("payment info" checkout step)
        /// </summary>
        /// <returns>View component type</returns>
        public Type GetPublicViewComponent()
        {
            return typeof(PaymentManualViewComponent);
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            //settings
            var settings = new ClickPaySettings
            {
                TransactMode = TransactMode.Pending
            };
            await _settingService.SaveSettingAsync(settings);

            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Payments.Manual.Instructions"] = "This payment method stores credit card information in database (it's not sent to any third-party processor). In order to store credit card information, you must be PCI compliant.",
                ["Plugins.Payments.Manual.Fields.AdditionalFee"] = "Additional fee",
                ["Plugins.Payments.Manual.Fields.AdditionalFee.Hint"] = "Enter additional fee to charge your customers.",
                ["Plugins.Payments.Manual.Fields.AdditionalFeePercentage"] = "Additional fee. Use percentage",
                ["Plugins.Payments.Manual.Fields.AdditionalFeePercentage.Hint"] = "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.",
                ["Plugins.Payments.Manual.Fields.TransactMode"] = "After checkout mark payment as",
                ["Plugins.Payments.Manual.Fields.TransactMode.Hint"] = "Specify transaction mode.",
                ["Plugins.Payments.Manual.PaymentMethodDescription"] = "Pay by credit / debit card"
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<ManualPaymentSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Payments.Manual");

            await base.UninstallAsync();
        }

        /// <summary>
        /// Gets a payment method description that will be displayed on checkout pages in the public store
        /// </summary>
        /// <remarks>
        /// return description of this payment method to be display on "payment method" checkout step. good practice is to make it localizable
        /// for example, for a redirection payment method, description may be like this: "You will be redirected to PayPal site to complete the payment"
        /// </remarks>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return await _localizationService.GetResourceAsync("Plugins.Payments.Manual.PaymentMethodDescription");
        }

       

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture => false;

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund => false;

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund => false;

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid => false;

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.Manual;

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo => false;

        #endregion
    }
}