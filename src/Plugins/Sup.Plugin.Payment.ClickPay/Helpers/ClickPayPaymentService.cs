using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Sup.Plugin.Payment.ClickPay.Helpers;
using Sup.Plugin.Payment.ClickPay.Models;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace Sup.Plugin.Payment.ClickPay.Helpers
{
    public class ClickPayPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClickPayPaymentSettings _clickPayPaymentSettings;

        public ClickPayPaymentService(IHttpClientFactory httpClientFactory, ClickPayPaymentSettings clickPayPaymentSettings)
        {
            _httpClientFactory = httpClientFactory;
            _clickPayPaymentSettings = clickPayPaymentSettings;
        } 
        

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var requestUrl = "https://secure.clickpay.com.sa/payment/request";

            var requestData = new
            {
                profile_id = _clickPayPaymentSettings.ProfileId,
                tran_type = "sale",
                tran_class = "ecom",
                cart_id = paymentRequest.CartId,
                cart_description = paymentRequest.CartDescription,
                cart_currency = paymentRequest.CartCurrency,
                cart_amount = paymentRequest.CartAmount,
                callback = _clickPayPaymentSettings.CallbackUrl,
                returnURl = _clickPayPaymentSettings.ReturnUrl,
                customer_details = paymentRequest.CustomerDetails
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(requestUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var paymentResult = JsonConvert.DeserializeObject<PaymentResult>(responseContent);
                return paymentResult;
            }
            else
            {
                // Handle unsuccessful API call
                throw new Exception($"Failed to process payment. Status code: {response.StatusCode}");
            }
        }

        public async Task<RefundResult> ProcessRefundAsync(string orderId, decimal amountToRefund)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var requestUrl = "https://secure.clickpay.com.sa/payment/refund";

            var requestData = new
            {
                profile_id = _clickPayPaymentSettings.ProfileId,
                order_id = orderId,
                refund_amount = amountToRefund
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(requestUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var refundResult = JsonConvert.DeserializeObject<RefundResult>(responseContent);
                return refundResult;
            }
            else
            {
                // Handle unsuccessful API call
                throw new Exception($"Failed to process refund. Status code: {response.StatusCode}");
            }
        }
    }
}

