using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sup.Plugin.Payment.ClickPay.Models
{
    public class PaymentRequest
    {
        public string CartId { get; set; }
        public string CartDescription { get; set; }
        public string CartCurrency { get; set; }
        public decimal CartAmount { get; set; }
        public string CallbackUrl { get; set; }
        public string ReturnUrl { get; set; }
        public CustomerDetails CustomerDetails { get; set; }
    }
}
