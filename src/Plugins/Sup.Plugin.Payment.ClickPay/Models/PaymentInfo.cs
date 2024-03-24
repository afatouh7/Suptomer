using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Sup.Plugin.Payment.ClickPay.Models
{
    public class PaymentInfo 
    {
        public PaymentInfo()
        {
            CreditCardTypes = new List<SelectListItem>();
            ExpireMonths = new List<SelectListItem>();
            ExpireYears = new List<SelectListItem>();
        }
        
        [NopResourceDisplayName("Payment.SelectCreditCard")]
        public string card_type { get; set; }
        [NopResourceDisplayName("Payment.SelectCreditCard")]
        public IList<SelectListItem> CreditCardTypes { get; set; }
        [NopResourceDisplayName("Payment.CardholderName")]
        public string card_scheme { get; set; }
        [NopResourceDisplayName("Payment.Description ")]
        public string payment_description { get; set; }
        [NopResourceDisplayName("Payment.ExpirationDate")]
        public string expiryYear { get; set; }
        [NopResourceDisplayName("Payment.ExpirationDate")]
        public string expiryMonth { get; set; }
        public IList<SelectListItem> ExpireMonths { get; set; }

        public IList<SelectListItem> ExpireYears { get; set; }


    }
}
